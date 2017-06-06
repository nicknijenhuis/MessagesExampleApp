using System;
using System.Collections.Generic;
using System.Linq;
using MessagesAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessagesAPI.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private static readonly List<Message> Messages = new List<Message>
        {
            new Message {Id = 1, Date = DateTime.UtcNow, Title = "Message 1", Description = "This is the first message"}
        };


        // GET api/messages
        [HttpGet]
        public IEnumerable<Message> Get() => Messages.OrderByDescending(x => x.Date);

        // GET api/messages/5
        [HttpGet("{id}")]
        public Message Get(int id) => Messages.SingleOrDefault(x => x.Id == id);

        // POST api/messages
        [HttpPost]
        public void Post([FromBody] Message message)
        {
            message.Id = Messages.Max(x => x.Id) + 1;
            message.Date = DateTime.UtcNow;
            Messages.Add(message);
        }

        // PUT api/messages/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/messages/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

