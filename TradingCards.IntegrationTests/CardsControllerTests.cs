using FluentAssertions;
using TradingCards.IntegrationTests.Factories;

namespace TradingCards.IntegrationTests
{
    public class CardsControllerTests
    {
        readonly TradingCardsFactory factory = new();

        [Fact]
        public async Task AutoComplete_ShouldReturnCardsFromBothCollection()
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync("/cards/autocomplete?query=max");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var data = await response.Content.ReadAsStringAsync();
            // should return cards from lorcana and mtg
            data.Should().BeEquivalentTo("""{"cards":{"type":"LORCANA","inkCost":3,"rarity":"Uncommon","id":"maximus-relentless-pursuer","name":"Maximus - Relentless Pursuer"},{"type":"LORCANA","inkCost":3,"rarity":"Common","id":"max-loyal-sheepdog","name":"Max - Loyal Sheepdog"},{"type":"LORCANA","inkCost":5,"rarity":"SuperRare","id":"maximus-palace-horse","name":"Maximus - Palace Horse"},{"type":"LORCANA","inkCost":6,"rarity":"SuperRare","id":"maximus-team-champion","name":"Maximus - Team Champion"},{"type":"MTG","color":"GREEN","rarity":"Uncommon","id":"gluetius-maximus-22dcba0a","name":"Gluetius Maximus"},{"type":"MTG","color":"BLUE","rarity":"Common","id":"maximize-altitude-70e8eba9","name":"Maximize Altitude"},{"type":"MTG","color":"RED","rarity":"Common","id":"maximize-velocity-96e7e4c8","name":"Maximize Velocity"},{"type":"MTG","color":"BLACK","rarity":"Rare","id":"elder-arthur-maxson-d95cd013","name":"Elder Arthur Maxson"}}""");
        }

        [Fact]
        public async Task Filter_WithGeneralFilter_ShouldReturnCardsFromBothCollections()
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync("/cards/search?name=max");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var data = await response.Content.ReadAsStringAsync();
            // should return cards from lorcana and mtg
            data.Should().BeEquivalentTo("""{"cards":{"type":"LORCANA","inkCost":3,"rarity":"Common","id":"max-loyal-sheepdog","name":"Max - Loyal Sheepdog"},{"type":"MTG","color":"WHITE","rarity":"Rare","id":"archaeomancers-map-ef833546","name":"Archaeomancer's Map"},{"type":"MTG","color":"WHITE","rarity":"Mythic","id":"land-tax-d2d9ecea","name":"Land Tax"},{"type":"MTG","color":"BLACK","rarity":"Rare","id":"mad-auntie-b582b696","name":"Mad Auntie"},{"type":"MTG","color":"RED","rarity":"Common","id":"mad-dog-097bce84","name":"Mad Dog"},{"type":"MTG","color":"RED","rarity":"Common","id":"mad-prophet-91081181","name":"Mad Prophet"},{"type":"MTG","color":"RED","rarity":"Uncommon","id":"mad-ratter-779fa24d","name":"Mad Ratter"},{"type":"MTG","color":"BLUE","rarity":"Common","id":"mer-man-47eefb06","name":"Mer Man"},{"type":"MTG","color":"WHITE","rarity":"Rare","id":"monologue-tax-ace8e88e","name":"Monologue Tax"},{"type":"MTG","color":null,"rarity":"Mythic","id":"mox-amber-7a43bd27","name":"Mox Amber"}}""");
        }

        [Fact]
        public async Task Filter_WithMtgFilter_ShouldReturnMtgCard()
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync("/cards/search?type=MTG&name=niv&color=Black&rarity=Rare");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var data = await response.Content.ReadAsStringAsync();
            // should return cards from lorcana and mtg
            data.Should().BeEquivalentTo("""{"cards":{"type":"MTG","color":"BLACK","rarity":"Rare","id":"niv-mizzet-guildpact-46570366","name":"Niv-Mizzet, Guildpact"},{"type":"MTG","color":"BLACK","rarity":"Rare","id":"niv-mizzet-supreme-4c3a865a","name":"Niv-Mizzet, Supreme"}}""");
        }

        [Fact]
        public async Task Filter_WithLorcanaFilter_ShouldReturnLorcanaCards()
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync("/cards/search?name=simba&type=lorcana&rarity=Rare&inkCost=7");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var data = await response.Content.ReadAsStringAsync();
            // should return cards from lorcana and mtg
            data.Should().BeEquivalentTo("""{"cards":{"type":"LORCANA","inkCost":7,"rarity":"Rare","id":"simba-returned-king","name":"Simba - Returned King"}}""");
        }
    }
}