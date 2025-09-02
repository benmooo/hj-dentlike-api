dev:
    dotnet watch --project src/Api run

# create solution 
create-sln:
    dotnet new sln -n Dentlike

# create project
create-projects:
    cd src && \
    dotnet new webapi -n Api && \
    dotnet new classlib -n Application && \
    dotnet new classlib -n Domain && \
    dotnet new classlib -n Infrastructure && \
    cd Tests && \
    dotnet new xunit -n Application.Tests && \
    dotnet new xunit -n Api.Tests && \
    dotnet new xunit -n Infrastructure.Tests

# add project references example
add-project-refs:
    dotnet add src/Api/Api.csproj reference src/Application/Application.csproj

# add package reference example 
add-package:
    dotnet add src/Api/Api.csproj package Swashbuckle.AspNetCore -v 6.4.0
    # if we already inside the project directory which is src/Api
    # dotnet add package Swashbuckle.AspNetCore -v 6.4

add-all-projects-to-sln:
    find src -name "*.csproj" | xargs -I {} dotnet sln add {}

setup-user-secrets key value:
    cd src/Api && \
    dotnet user-secrets init && \
    dotnet user-secrets set "{{key}}" "{{value}}"
    # eg. dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=MyProjectDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;MultipleActiveResultSets=True"

install-ef-tools:
    dotnet tool install --global dotnet-ef
    dotnet ef --version

create-migration name:
    dotnet ef migrations add {{name}} \
        -p src/Infrastructure \
        -s src/Api \
        -o Data/Migrations

migration-apply:
    dotnet ef database update \
        -p src/Infrastructure \
        -s src/Api

# roll back the last applied migration
migration-down:
    dotnet ef migrations remove \
        -p src/Infrastructure \
        -s src/Api

# roll back to a specific migration
migration-rollback name:
    dotnet ef database update {{name}} \
        -p src/Infrastructure \
        -s src/Api

# list migrations status
migration-list:
    dotnet ef migrations list \
        -p src/Infrastructure \
        -s src/Api
    
# generate SQL script for migrations
# Note: 在部署到生产环境时，通常不会直接运行 database update，而是生成一个 SQL 脚本交给 DBA 审核和执行
migration-script from="0":
    dotnet ef migrations script {{from}} \
        -p src/Infrastructure \
        -s src/Api \
        -o migration.sql