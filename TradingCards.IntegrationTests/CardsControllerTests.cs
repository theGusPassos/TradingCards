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
            data.Should().BeEquivalentTo("""{"cards":[{"type":"LORCANA","id":"maximus-relentless-pursuer","name":"Maximus - Relentless Pursuer"},{"type":"LORCANA","id":"max-loyal-sheepdog","name":"Max - Loyal Sheepdog"},{"type":"LORCANA","id":"maximus-palace-horse","name":"Maximus - Palace Horse"},{"type":"LORCANA","id":"maximus-team-champion","name":"Maximus - Team Champion"},{"type":"MTG","id":"gluetius-maximus-22dcba0a","name":"Gluetius Maximus"},{"type":"MTG","id":"maximize-altitude-70e8eba9","name":"Maximize Altitude"},{"type":"MTG","id":"maximize-velocity-96e7e4c8","name":"Maximize Velocity"},{"type":"MTG","id":"elder-arthur-maxson-d95cd013","name":"Elder Arthur Maxson"}]}""");
        }

        [Fact]
        public async Task Filter_WithGeneralFilter_ShouldReturnCardsFromBothCollections()
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync("/cards/search?name=max");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var data = await response.Content.ReadAsStringAsync();
            // should return cards from lorcana and mtg
            data.Should().BeEquivalentTo("""{"cards":[{"type":"LORCANA","id":"max-loyal-sheepdog","name":"Max - Loyal Sheepdog"},{"type":"MTG","id":"archaeomancers-map-ef833546","name":"Archaeomancer's Map"},{"type":"MTG","id":"land-tax-d2d9ecea","name":"Land Tax"},{"type":"MTG","id":"mad-auntie-b582b696","name":"Mad Auntie"},{"type":"MTG","id":"mad-dog-097bce84","name":"Mad Dog"},{"type":"MTG","id":"mad-prophet-91081181","name":"Mad Prophet"},{"type":"MTG","id":"mad-ratter-779fa24d","name":"Mad Ratter"},{"type":"MTG","id":"mer-man-47eefb06","name":"Mer Man"},{"type":"MTG","id":"monologue-tax-ace8e88e","name":"Monologue Tax"},{"type":"MTG","id":"mox-amber-7a43bd27","name":"Mox Amber"}]}""");
        }

        [Fact]
        public async Task Filter_WithMtgFilter_ShouldReturnMtgCard()
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync("/cards/search?name=niv&color=");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var data = await response.Content.ReadAsStringAsync();
            // should return cards from lorcana and mtg
            data.Should().BeEquivalentTo("""{"cards":[{"type":"LORCANA","id":"max-loyal-sheepdog","name":"Max - Loyal Sheepdog"},{"type":"MTG","id":"archaeomancers-map-ef833546","name":"Archaeomancer's Map"},{"type":"MTG","id":"land-tax-d2d9ecea","name":"Land Tax"},{"type":"MTG","id":"mad-auntie-b582b696","name":"Mad Auntie"},{"type":"MTG","id":"mad-dog-097bce84","name":"Mad Dog"},{"type":"MTG","id":"mad-prophet-91081181","name":"Mad Prophet"},{"type":"MTG","id":"mad-ratter-779fa24d","name":"Mad Ratter"},{"type":"MTG","id":"mer-man-47eefb06","name":"Mer Man"},{"type":"MTG","id":"monologue-tax-ace8e88e","name":"Monologue Tax"},{"type":"MTG","id":"mox-amber-7a43bd27","name":"Mox Amber"}]}""");
        }
    }
}