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