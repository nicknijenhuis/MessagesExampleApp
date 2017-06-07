using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using MessagesApp.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(MessagesApp.Services.MessagesDataStore))]
namespace MessagesApp.Services
{
    public class MessagesDataStore : IDataStore<Item>
    {
        List<Item> items;

        public async Task<bool> AddItemAsync(Item item)
        {
            using (var client = new HttpClient())
            {
                var jsonString = JsonConvert.SerializeObject(new { item.Title, item.Description });
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://messageswebapi.azurewebsites.net/api/messages", content);
                response.EnsureSuccessStatusCode();
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            await InitializeAsync();

            var _item = items.FirstOrDefault(arg => arg.Id == item.Id);
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Item item)
        {
            await InitializeAsync();

            var _item = items.FirstOrDefault(arg => arg.Id == item.Id);
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            await InitializeAsync();

            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeAsync();

            return await Task.FromResult(items);
        }

        public Task<bool> PullLatestAsync() => Task.FromResult(true);

        public Task<bool> SyncAsync() => Task.FromResult(true);

        public async Task InitializeAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync("http://messageswebapi.azurewebsites.net/api/messages");

                items = JsonConvert.DeserializeObject<List<Item>>(response);
            }
        }
    }
}
