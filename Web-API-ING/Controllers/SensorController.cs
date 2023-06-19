using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Web_API_ING.Data;
using Web_API_ING.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization;
using System.Globalization;
using Microsoft.AspNetCore.SignalR;


namespace Web_API_ING.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SensorController : ControllerBase
	{
		private readonly IMongoCollection<TempObj> _mongoClient;
		private readonly IMongoCollection<TempAlive> _mongoAlive;
    

        public SensorController(IMongoClient mongoClient)
		{
			IMongoDatabase database = mongoClient.GetDatabase("IOT");
			_mongoClient = database.GetCollection<TempObj>("temperature_humidity");
			_mongoAlive = database.GetCollection<TempAlive>("temperature_humidity_service");
      
        }



        [HttpPost]
		[Route("SentSensor")]
		public async Task<IActionResult> SentSensor([FromBody] TempObj sensor)
		{
			try
			{
			   	await _mongoClient.InsertOneAsync(sensor);
				return Ok(sensor);
			}
			catch(Exception ex)
			{ return BadRequest(ex); }
		}

		[HttpPost]
		[Route("PCAlive")]
		public async Task<IActionResult> PCAlive([FromBody] TempAlive ObjPC)
		{
			try
			{


				TempAlive document = new TempAlive();
				var filter = Builders<TempAlive>.Filter.Eq("PC",(ObjPC.PC));
				document  = await _mongoAlive.Find(filter).FirstOrDefaultAsync();
				if (document == null)
				{
					await _mongoAlive.InsertOneAsync(ObjPC);
				}
				else
				{
					
					var filters = Builders<TempAlive>.Filter.Eq("PC", ObjPC.PC);

					var update = Builders<TempAlive>.Update.Set(x => x.LastUpdate, ObjPC.LastUpdate);

					var result = await _mongoAlive.UpdateOneAsync(filter, update);

					if (result.ModifiedCount == 0)
						return NotFound(); // Document not found

				}
				return Ok(document);
			}
			catch (Exception ex) { return BadRequest(ex); }
		}


		//[HttpPost]
		//[Route("GetSensor")]
		//public async Task GetSensorWithLine([FromBody] string PC)
		//{
		//	 RealTimeDataHub ch = new RealTimeDataHub();
	 // 	await	ch.(PC, PC);


  //      }
	}
}
