# Running the Project

```
cd scripts
docker-compose up
```

Then its possible to run and debug through Visual Studio or run `dotnet run --project TradingCards`.

## Endopoints

`/cards/autocomplete?query=`

And the filter: `/cards/filter?`

It can be generic with `filter?name=` for all cards or explicit with a type `filter?type=mtg&color=blue`

# Running Integration Tests

Requirements:
- the container must be running
- run the app before the tests

```
dotnet test
```
