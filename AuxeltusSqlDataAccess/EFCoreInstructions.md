# Entity Framework Core Usage Recommendations

AuxeltusSqlDataAccess is a .NetStandard 2.1 project that utilizes the latest supported EF Core version (5.0.17). Follow the instructions included making changes that need to be propagated to the SQL.

## Generating Migrations

Ensure that you've properly updated AuxeltusSqlContext with the requisite changes that require a new migration, then use the following CLI command (once in AuxeltusSqlDataAccess directory).

**dotnet ef migrations add NAME_OF_MIGRATION_HERE --startup-project --startup-project ../AuxeltusSqlDataAccessTests**

The above command leverages the unit testing project (AueltusSqlDataAccessTests) to generate a migration, that would not be possible with a .NetStandard project alone.

## Generating Scripts

While this project uses a 'code-first' SQL design pattern, we do not rely on the execution of migrations through code to make modifications to the SQL structure; instead, we generate scripts that are then executed against the SQL server itself. These scripts should be added to the 'Scripts' folder in this access layer. They should be numbered incrementally, and named after the corresponding migration. With that said, they also should be created idempotently, such that the most recent script includes every migration.

**dotnet ef migrations script --startup-project ../AuxeltusSqlDataAccessTests -i -o C:\SourceControl\DotNet\Auxeltus\AuxeltusSqlDataAccess\Scripts\##_NAME_OF_MIGRATION_HERE.sql**