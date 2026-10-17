# Dynamic Product, Category, and Status translations

## Implementation and migration state

The source model and `Tracker.dbml` now describe one generic translation system
for Product, Category, and Status. Product no longer has fixed Arabic/English
properties. No ProductTranslation, CategoryTranslation, StatusTranslation, or
TranslationKey lookup table is required.

The applied `20261017185206_InitialCreate` is the single active and applied migration.
Its designer and snapshot, `Database/InitialCreate.sql`, `Tracker.dbml`, and the live
PostgreSQL catalog were independently verified against the EF model. No model changes
are pending. `Program.cs` checks connectivity and does not automatically apply migrations.

| Verified final measure | Count |
| --- | ---: |
| Application tables | 21 |
| Columns | 140 |
| Primary keys | 21 |
| Alternate UNIQUE keys | 1 |
| Foreign keys | 30 |
| CHECK constraints | 50 |
| Physical indexes, including keys and UX_User_Email | 60 |
| Initial languages / statuses / translations | 2 / 12 / 24 |
| Applied migrations | 1 |

Counts exclude `__EFMigrationsHistory` and PostgreSQL's internal NOT NULL catalog
entries. They were independently calculated from EF metadata, the generated migration,
the SQL reference instance, and the live PostgreSQL 18 catalog.

## Files changed

- `Models/Product.cs`: replace four bilingual fields with two translation keys.
- `Data/TrackerDbContext.cs`: Product key mappings, three translation CHECKs, and
  one unique name-key index. Other entity mappings and seeds remain unchanged.
- `Database/Tracker.dbml`: updated Product and target counts; all existing
  relationships remain represented.
- `Migrations/20261017185206_InitialCreate.cs` and its designer: one fresh initial
  migration creating the final schema directly, with explicit UX_User_Email SQL.
- `Migrations/TrackerDbContextModelSnapshot.cs`: regenerated final model snapshot.
- `Database/InitialCreate.sql`: regenerated from the new migration.
- `Database/TranslationDesign.md`: architecture, usage, validation, and completed
  consolidation record.

The old `20261017175841_InitialCreate` migration/designer were removed from the active
project; their verified source backups remain intact. No unrelated entities,
application logic, project dependencies, or business rules were changed.

## Data model

Language has an integer identity LanguageId, unique required VARCHAR(10) Code,
required VARCHAR(100) Name, and IsActive defaulting to true. Initial languages are
Arabic (`ar`, ID 1) and English (`en`, ID 2). These are seed records, not a limit on
supported languages. User.PreferredLanguageId remains an optional RESTRICT FK.

Translation has an integer identity TranslationId, required VARCHAR(200) Key,
required TEXT Value, and required LanguageId with RESTRICT deletion.
`UNIQUE(key, language_id)` provides one translation per key and language. Existing
nonblank key/value CHECKs and the language index remain intact.

Product now has:

| Property | PostgreSQL mapping |
| --- | --- |
| ProductId | integer identity primary key |
| NameTranslationKey | VARCHAR(200), NOT NULL, UNIQUE, contains a non-whitespace character |
| DescriptionTranslationKey | nullable VARCHAR(200); contains a non-whitespace character when present |
| Brand | nullable VARCHAR(150) |
| ImageUrl | nullable TEXT |
| Specifications | nullable JSONB object |
| IsActive | required boolean, default true |
| CreatedAt / UpdatedAt | required timestamps with time zone, default now() |

The keys must differ when DescriptionTranslationKey is present. Product date and
JSON-object constraints, primary key, ProductVariant/ProductCategory/ShareLink
relationships, and all navigation properties are preserved. No key has a database
default. The Product key CHECKs reject spaces, tabs, and newlines using a
PostgreSQL whitespace character class.

Category retains its required unique NameTranslationKey, optional description
key, hierarchy, unique slug, active flag, ProductCategory relationship, and
self-parent/delete restrictions.

Status retains numeric StatusId, Scope, Code, TranslationKey, active flag, and sort
order. Its `(scope, code)` and translation key are unique. Each workflow's
`(status_id, status_scope)` FK references `Status(status_id, scope)`, and a CHECK
fixes its scope to crawl_run, listing_match, or affiliate_conversion. This prevents
wrong-workflow IDs and spoofed scopes. Codes remain configurable data, and workflow
records have no numeric status default. Resolve IDs by `(scope, code)`.

There is no FK from a Product/Category/Status translation key to Translation.Key:
that column intentionally repeats across languages. The application owns these
logical references and required-language coverage.

## Key generation and atomic creation

Generate a UUID **before** inserting a Product. Use one stable token for that
Product, for example `product_<32 hexadecimal UUID characters>_name` and
`product_<same token>_description`. Do not depend on the identity ProductId being
available, and keep keys stable when names, brands, or language preferences change.
The namespace and UUID resist collisions; database uniqueness is the final guard.
Use separate category_ and workflow namespaces for the other owners.

Application code must validate the configured required language codes, language
activation, nonblank supplied values, distinct key ownership, and absence of
duplicate language entries before saving. Required-language policy is application
configuration, not a hardcoded list of languages or numeric IDs in Product.

An insertion service can use this pattern after validating its input dictionaries
`namesByCode` and `descriptionsByCode` against that policy:

```csharp
string token = Guid.NewGuid().ToString("N");
var product = new Product
{
    NameTranslationKey = $"product_{token}_name",
    DescriptionTranslationKey = descriptionsByCode.Count == 0
        ? null : $"product_{token}_description"
};

await using var transaction = await context.Database.BeginTransactionAsync();
var languageIds = await context.Languages
    .Where(l => l.IsActive)
    .ToDictionaryAsync(l => l.Code, l => l.LanguageId);

context.Products.Add(product);
foreach (var (code, value) in namesByCode)
    context.Translations.Add(new Translation
    {
        Key = product.NameTranslationKey,
        LanguageId = languageIds[code],
        Value = value
    });
foreach (var (code, value) in descriptionsByCode)
    context.Translations.Add(new Translation
    {
        Key = product.DescriptionTranslationKey!,
        LanguageId = languageIds[code],
        Value = value
    });

await context.SaveChangesAsync();
await transaction.CommitAsync();
```

This is an application integration example; the current console program has no
Product creation API. The disposable tests execute the equivalent EF insertion
and lookups. Validate values with `string.IsNullOrWhiteSpace` at the service boundary
as well as relying on database constraints. Reject unknown/inactive language codes
with a validation result before the dictionary lookups above.

## Retrieval, another language, and missing translations

Resolve the Product's name or description key, then look up `(key, language)`.
Use the same lookup for Category and Status. For example, with application-configured
`requestedCode` and `fallbackCode`:

```csharp
string? value = await context.Translations
    .Where(t => t.Key == key && t.Language.IsActive
        && (t.Language.Code == requestedCode || t.Language.Code == fallbackCode))
    .OrderBy(t => t.Language.Code == requestedCode ? 0 : 1)
    .Select(t => t.Value)
    .FirstOrDefaultAsync();
```

If both lookups are missing, use an appropriate localized unavailable-name message
and log the missing key internally. An absent optional description requires no
lookup or placeholder. Avoid presenting internal translation keys to customers.
Validate and create required translations atomically so missing names are exceptional.

To add French, insert an active Language record with Code `fr`, resolve its ID,
and add Translation rows for the existing Product, Category, and Status keys.
No Product column or schema change is needed. Additional languages and their
coverage are maintained as database records and application policy.

## Database enforcement and application responsibilities

The database enforces field requiredness/lengths; Product name-key uniqueness;
nonblank Product keys; distinct Product name/description keys; translation
key/language uniqueness; existing translation key/value CHECKs; Language FKs;
all existing catalog relationships; workflow status scopes; and case-insensitive
user email uniqueness.

The application must ensure referenced translation keys exist, belong to the
correct entity namespace/owner, and cover required active languages. It must also
coordinate translation edits and cleanup with owner changes/deletion, prevent
removing required translations while owners still reference them, validate inputs,
and apply missing-translation fallback. These cross-record rules are not FKs to
Translation.Key and are not falsely claimed as database-enforced.

Only `UQ_Product_NameTranslationKey` is added. It enforces required uniqueness;
there is no additional plain index on the same column, no description-key index,
and no redundant FK or translation lookup table. `UX_User_Email` on LOWER(email)
is explicitly included in the applied InitialCreate and generated SQL because EF's
ordinary property indexes do not represent it.

## Intentionally nontranslated text

- User names, saved collection names/descriptions, and user-entered content remain
  their original text.
- Merchant names, Product brands, and Language native names are proper names.
- Variant codes, GTINs, slugs, URLs, currencies, external IDs, tracking codes,
  timestamps, and workflow scope/code identifiers are structured data.
- CrawledListing.ProductName and raw crawler JSON retain the source merchant's data;
  internal error messages remain diagnostics.
- Specifications and variant/offer attributes remain existing JSON data. No explicit
  requirement authorizes redesigning or translating each JSON value.
- Notification title/message remain rendered delivery content. Creation should use
  the recipient's language; changing old notifications' display language would be
  a separate requirement for template/event data.

## Backups and approved consolidation

Before the approved administrator reset, read-only SQL on 2026-10-17 independently
found zero rows in all 18 business tables, including Product and Category. The
2 languages, 12 statuses, and 24 status translations were backed up and preserved
exactly; there was no Product text requiring transfer.

Verified backups remain private under:

- `.backups/product-translations-20261017T183038156675Z/`: original source archive,
  per-file hashes, read-only preflight, and custom-format PostgreSQL dump/hash.
- `.backups/approved-product-consolidation-20261017T184757523687Z/`: all 31 prepared
  source files before consolidation, archive/hash manifest, and a fresh read-only
  zero-business-data/seed preflight.

All 9 existing backup files were hash-verified before and after consolidation.
The database dump's archive listing and full SQL decoding succeeded. Source archives
were checked against every recorded per-file SHA-256. These checks do not claim a
restore rehearsal. No backup was overwritten or deleted; credentials were excluded.

The user approved resetting only development tracker_db and recreated it using the
PostgreSQL administrator account. The agent independently verified zero user tables,
views, sequences, or migration history and confirmed tracker_user could create tables.
The agent did not reset the database again.

After verifying backups, the old active migration/designer/snapshot were removed,
the project was rebuilt without a restore, and EF generated one new InitialCreate.
UX_User_Email was explicitly added, the migration was rebuilt and inspected, and
InitialCreate.sql was generated from it. The migration was then applied successfully.
No historical files were renamed or migration IDs fabricated.

## Final verification results

| Verification | Result |
| --- | --- |
| Build | PASS: zero warnings and errors |
| Pending EF changes | PASS: no changes since the migration |
| Applied migration | PASS: exactly 20261017185206_InitialCreate, EF 10.0.13 |
| EF vs migration | PASS: every table, column, type, default, identity, key, FK, CHECK, index, and seed field matches |
| SQL vs migration | PASS: exact EF-generated script, allowing BOM/newline framing only |
| DBML vs EF | PASS: all tables, columns/types/nullability/defaults, identities, keys/indexes, CHECKs, FKs and delete actions match |
| Live catalog vs SQL reference | PASS: all 21 tables, 140 columns, 102 key/FK/CHECK constraints, and 60 indexes match |
| Product structure | PASS: nine final columns; no name_en, name_ar, description_en, or description_ar |
| Reference seeds | PASS: all 2 languages, 12 statuses and 24 translations match every EF field and the preserved pre-reset records |
| Expression comparator | PASS: 15 regressions, including equivalent TRIM forms and distinct direction/characters/grouping/literals/casts |
| Arabic/English Product | PASS: name and description lookups |
| Third language | PASS: French Product names/descriptions without schema changes |
| Category / Status | PASS: Arabic, English and French lookups |
| Key creation | PASS: UUID keys generated before ProductId; Product and translation rows saved atomically |
| Other catalog relationships | PASS: ProductVariant, ProductCategory and merchant/offer joins, specifications and attributes |
| NOT NULL rejection | PASS: 106 required-column cases |
| Primary-key rejection | PASS: 21 duplicate-primary-key cases |
| Foreign-key rejection | PASS: 30 missing-reference cases |
| CHECK rejection | PASS: all 50 CHECKs, including Product distinct keys and workflow fixed scopes |
| Unique-index rejection | PASS: 14 cases, including Product name keys and mixed-case duplicate email |
| Wrong-workflow statuses | PASS: three existing-but-wrong-scope ID cases |
| Extra Product whitespace tests | PASS: six empty/space/tab/newline-only name/description key cases |
| Optional description | PASS: NULL accepted |
| Backups and unrelated source | PASS: preserved and hash-checked |

All 224 systematic rejection cases and six additional Product whitespace cases
passed. Valid fixtures for all 21 application tables and the dynamic translation
scenarios also passed. Data-changing tests ran only in a separate disposable
PostgreSQL instance created from the generated InitialCreate.sql. Transactions and
savepoints rolled all test data back; catalog and seed records matched before/after.
The live verification transaction was READ ONLY. No test records were added to
tracker_db, and the disposable server was stopped afterward.

The compiler, parsed-expression comparator, migration-operation audit and database
validation tools remain under `/tmp/tracker-product-translation-validation/`.
Detailed final catalog and test results are in
`/tmp/tracker-product-final-verification-fyaidn05/verification-results.json`.
No runtime dependencies were added. No further reset, approval, or migration action
is pending. No commit or GitHub push was performed.
