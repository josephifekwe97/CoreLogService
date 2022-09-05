using System;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json;

namespace Core.LogService.Models
{

    public class Dto
    {

    } 
    public class updateModel<T> : mongodbmodel
    {
        public Dictionary<string, string> filter { get; set; }
        public T update { get; set; }
        //public Update update { get; set; }
    }
    public class Update
    {
        [JsonProperty("$set")]
        public Dictionary<string,string> Set { get; set; }
    }

    public class mongodbmodel
    {
        public string collection { get; set; }
        public string database { get; set; }
        public string dataSource { get; set; }
    }

    public class filterModel : mongodbmodel
    {
        public object filter { get; set; }
    }

    public class saveModel<T> : mongodbmodel
    {
        public T document { get; set; }
    }



    public class deleteModel : mongodbmodel
    {
        public JsonElement filter { get; set; }
    }

    public class filterResponse
    {
        public List<object> documents { get; set; }
    }

    public class masterObj
    {
        public string collection { get; set; }
    }
    public class filterObj : masterObj
    {
        public object filter { get; set; }
    }


    public class saveObj : masterObj
    {

        public object document { get; set; }
    }

    public class updateObj : masterObj
    {
        public object filter { get; set; }
        public object document { get; set; }
    }
}
