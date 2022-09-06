using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.LogService.Data;
using Core.LogService.Interface;
using Microsoft.Extensions.Configuration;

namespace Core.LogService.Services
{
    public class MongoDbService : ILogService
    {
        public IConfiguration _configuration;

        public MongoDbService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Models.filterResponse> FindLog(string filter, string collection = "")
        {
            try
            {
                collection = string.IsNullOrEmpty(collection) ? _configuration.GetValue<string>("LogService:DefaultCollectionName") : collection;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration.GetValue<string>("MongoDb:BaseLogUrl"));

                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add("api-key", _configuration.GetValue<string>("MongoDb:api-key"));

                    var payload = new Models.filterModel();

                    payload.collection = collection;

                    payload.database = _configuration.GetValue<string>("MongoDb:database");

                    payload.dataSource = _configuration.GetValue<string>("MongoDb:dataSource");

                    payload.filter = JsonSerializer.Deserialize<dynamic>(filter);

                    var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("find", content);

                    var responseString = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<Models.filterResponse>(responseString);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<bool> DeleteLog(string filter, string collection = "")
        {
            try
            {
                collection = string.IsNullOrEmpty(collection) ? _configuration.GetValue<string>("LogService:DefaultCollectionName") : collection;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration.GetValue<string>("MongoDb:BaseLogUrl"));

                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add("api-key", _configuration.GetValue<string>("MongoDb:api-key"));

                    var payload = new Models.filterModel();

                    payload.collection = collection;

                    payload.database = _configuration.GetValue<string>("MongoDb:database");

                    payload.dataSource = _configuration.GetValue<string>("MongoDb:dataSource");

                    payload.filter = JsonSerializer.Deserialize<dynamic>(filter);

                    var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("deleteOne", content);

                    var responseString = await response.Content.ReadAsStringAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> SaveLog(string data, string collection = "")
        {
            try
            {
                collection = string.IsNullOrEmpty(collection) ? _configuration.GetValue<string>("LogService:DefaultCollectionName") : collection;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration.GetValue<string>("MongoDb:BaseLogUrl"));

                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add("api-key", _configuration.GetValue<string>("MongoDb:api-key"));

                    var payload = new Models.saveModel<object>();

                    payload.collection = collection;

                    payload.database = _configuration.GetValue<string>("MongoDb:database");

                    payload.dataSource = _configuration.GetValue<string>("MongoDb:dataSource");

                    payload.document = JsonSerializer.Deserialize<dynamic>(data);

                    var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("insertOne", content);

                    var responseString = await response.Content.ReadAsStringAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> UpdateLog(string filter, string document, string collection = "")
        {
            try
            {
                collection = string.IsNullOrEmpty(collection) ? _configuration.GetValue<string>("LogService:DefaultCollectionName") : collection;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration.GetValue<string>("MongoDb:BaseLogUrl"));

                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add("api-key", _configuration.GetValue<string>("MongoDb:api-key"));

                    var payload = new Models.updateModel<object>();

                    payload.collection = collection;

                    payload.database = _configuration.GetValue<string>("MongoDb:database");

                    payload.dataSource = _configuration.GetValue<string>("MongoDb:dataSource");

                    payload.filter = JsonSerializer.Deserialize<Dictionary<string, string>>(filter);

                    payload.update = JsonSerializer.Deserialize<dynamic>(document);

                    //payload.update = new Models.Update()
                    //{
                    //    Set = JsonSerializer.Deserialize<Dictionary<string, string>>(data)
                    //};

                    var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                    var response = await client.PutAsync("findOneAndUpdate", content);

                    var responseString = await response.Content.ReadAsStringAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
