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