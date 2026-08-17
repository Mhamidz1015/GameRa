# ============================
# Base Runtime
# ============================

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app

EXPOSE 8080
EXPOSE 8081

# ============================
# Build
# ============================

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# ----------------------------
# API
# ----------------------------

COPY ["src/API/GameRa/GameRa.csproj","src/API/GameRa/"]

# ----------------------------
# Common
# ----------------------------

COPY ["src/Common/GameRa.Common.Application/GameRa.Common.Application.csproj","src/Common/GameRa.Common.Application/"]
COPY ["src/Common/GameRa.Common.Domain/GameRa.Common.Domain.csproj","src/Common/GameRa.Common.Domain/"]
COPY ["src/Common/GameRa.Common.Infrastructure/GameRa.Common.Infrastructure.csproj","src/Common/GameRa.Common.Infrastructure/"]
COPY ["src/Common/GameRa.Common.Presentation/GameRa.Common.Presentation.csproj","src/Common/GameRa.Common.Presentation/"]

# ----------------------------
# Games
# ----------------------------

COPY ["src/Modules/Games/GameRa.Modules.Games.Application/GameRa.Modules.Games.Application.csproj","src/Modules/Games/GameRa.Modules.Games.Application/"]
COPY ["src/Modules/Games/GameRa.Modules.Games.Domain/GameRa.Modules.Games.Domain.csproj","src/Modules/Games/GameRa.Modules.Games.Domain/"]
COPY ["src/Modules/Games/GameRa.Modules.Games.Infrastructure/GameRa.Modules.Games.Infrastructure.csproj","src/Modules/Games/GameRa.Modules.Games.Infrastructure/"]
COPY ["src/Modules/Games/GameRa.Modules.Games.Presentation/GameRa.Modules.Games.Presentation.csproj","src/Modules/Games/GameRa.Modules.Games.Presentation/"]
COPY ["src/Modules/Games/GameRa.Modules.Games.IntegrationEvents/GameRa.Modules.Games.IntegrationEvents.csproj","src/Modules/Games/GameRa.Modules.Games.IntegrationEvents/"]

# ----------------------------
# Store
# ----------------------------

COPY ["src/Modules/Store/GameRa.Modules.Store.Application/GameRa.Modules.Store.Application.csproj","src/Modules/Store/GameRa.Modules.Store.Application/"]
COPY ["src/Modules/Store/GameRa.Modules.Store.Domain/GameRa.Modules.Store.Domain.csproj","src/Modules/Store/GameRa.Modules.Store.Domain/"]
COPY ["src/Modules/Store/GameRa.Modules.Store.Infrastructure/GameRa.Modules.Store.Infrastructure.csproj","src/Modules/Store/GameRa.Modules.Store.Infrastructure/"]
COPY ["src/Modules/Store/GameRa.Modules.Store.Presentation/GameRa.Modules.Store.Presentation.csproj","src/Modules/Store/GameRa.Modules.Store.Presentation/"]
COPY ["src/Modules/Store/GameRa.Modules.Store.IntegrationEvents/GameRa.Modules.Store.IntegrationEvents.csproj","src/Modules/Store/GameRa.Modules.Store.IntegrationEvents/"]

# ----------------------------
# Users
# ----------------------------

COPY ["src/Modules/User/GameRa.Modules.Users.Application/GameRa.Modules.Users.Application.csproj","src/Modules/User/GameRa.Modules.Users.Application/"]
COPY ["src/Modules/User/GameRa.Modules.Users.Domain/GameRa.Modules.Users.Domain.csproj","src/Modules/User/GameRa.Modules.Users.Domain/"]
COPY ["src/Modules/User/GameRa.Modules.Users.Infrastructure/GameRa.Modules.Users.Infrastructure.csproj","src/Modules/User/GameRa.Modules.Users.Infrastructure/"]
COPY ["src/Modules/User/GameRa.Modules.Users.Presentation/GameRa.Modules.Users.Presentation.csproj","src/Modules/User/GameRa.Modules.Users.Presentation/"]
COPY ["src/Modules/User/GameRa.Modules.Users.IntegrationEvents/GameRa.Modules.Users.IntegrationEvents.csproj","src/Modules/User/GameRa.Modules.Users.IntegrationEvents/"]

# ----------------------------
# Library
# ----------------------------

COPY ["src/Modules/Library/GameRa.Modules.Library.Application/GameRa.Modules.Library.Application.csproj","src/Modules/Library/GameRa.Modules.Library.Application/"]
COPY ["src/Modules/Library/GameRa.Modules.Library.Domain/GameRa.Modules.Library.Domain.csproj","src/Modules/Library/GameRa.Modules.Library.Domain/"]
COPY ["src/Modules/Library/GameRa.Modules.Library.Infrastructure/GameRa.Modules.Library.Infrastructure.csproj","src/Modules/Library/GameRa.Modules.Library.Infrastructure/"]
COPY ["src/Modules/Library/GameRa.Modules.Library.Presentation/GameRa.Modules.Library.Presentation.csproj","src/Modules/Library/GameRa.Modules.Library.Presentation/"]
COPY ["src/Modules/Library/GameRa.Modules.Library.IntegrationEvents/GameRa.Modules.Library.IntegrationEvents.csproj","src/Modules/Library/GameRa.Modules.Library.IntegrationEvents/"]

# ----------------------------
# Discounts
# ----------------------------

COPY ["src/Modules/Discounts/GameRa.Modules.Discounts.Application/GameRa.Modules.Discounts.Application.csproj","src/Modules/Discounts/GameRa.Modules.Discounts.Application/"]
COPY ["src/Modules/Discounts/GameRa.Modules.Discounts.Domain/GameRa.Modules.Discounts.Domain.csproj","src/Modules/Discounts/GameRa.Modules.Discounts.Domain/"]
COPY ["src/Modules/Discounts/GameRa.Modules.Discounts.Infrastructure/GameRa.Modules.Discounts.Infrastructure.csproj","src/Modules/Discounts/GameRa.Modules.Discounts.Infrastructure/"]
COPY ["src/Modules/Discounts/GameRa.Modules.Discounts.Presentation/GameRa.Modules.Discounts.Presentation.csproj","src/Modules/Discounts/GameRa.Modules.Discounts.Presentation/"]
COPY ["src/Modules/Discounts/GameRa.Modules.Discounts.IntegrationEvents/GameRa.Modules.Discounts.IntegrationEvents.csproj","src/Modules/Discounts/GameRa.Modules.Discounts.IntegrationEvents/"]

# ----------------------------
# Reviews
# ----------------------------

COPY ["src/Modules/Reviews/GameRa.Modules.Reviews.Application/GameRa.Modules.Reviews.Application.csproj","src/Modules/Reviews/GameRa.Modules.Reviews.Application/"]
COPY ["src/Modules/Reviews/GameRa.Modules.Reviews.Domain/GameRa.Modules.Reviews.Domain.csproj","src/Modules/Reviews/GameRa.Modules.Reviews.Domain/"]
COPY ["src/Modules/Reviews/GameRa.Modules.Reviews.Infrastructure/GameRa.Modules.Reviews.Infrastructure.csproj","src/Modules/Reviews/GameRa.Modules.Reviews.Infrastructure/"]
COPY ["src/Modules/Reviews/GameRa.Modules.Reviews.Presentation/GameRa.Modules.Reviews.Presentation.csproj","src/Modules/Reviews/GameRa.Modules.Reviews.Presentation/"]
COPY ["src/Modules/Reviews/GameRa.Modules.Reviews.IntegrationEvents/GameRa.Modules.Reviews.IntegrationEvents.csproj","src/Modules/Reviews/GameRa.Modules.Reviews.IntegrationEvents/"]

RUN dotnet restore "src/API/GameRa/GameRa.csproj"

COPY . .

WORKDIR "/src/src/API/GameRa"

RUN dotnet build "GameRa.csproj" -c Release -o /app/build

# ============================
# Publish
# ============================

FROM build AS publish

RUN dotnet publish "GameRa.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ============================
# Runtime
# ============================

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

COPY --from=build /src/src/API/GameRa/modules.games.json .
COPY --from=build /src/src/API/GameRa/modules.LibraryItem.json .
COPY --from=build /src/src/API/GameRa/modules.store.json .
COPY --from=build /src/src/API/GameRa/modules.users.json .
COPY --from=build /src/src/API/GameRa/modules.Discounts.json .
COPY --from=build /src/src/API/GameRa/modules.Reviews.json .

ENTRYPOINT ["dotnet","GameRa.dll"]