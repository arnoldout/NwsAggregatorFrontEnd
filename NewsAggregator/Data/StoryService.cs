﻿using NewsAggregator.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Data
{
    class StoryService
    {
        public static async Task<List<Story>> getStories(String id)
        {
            //get all stories linked to user's account
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Story));
            String url = App.apiURL + "getArticles/" + id;
            WebRequest wrGETURL = WebRequest.Create(url);
            wrGETURL.Proxy = null;

            WebResponse response = await wrGETURL.GetResponseAsync();
            Stream dataStream = response.GetResponseStream();
            StreamReader objReader = new StreamReader(dataStream);
            dynamic dynStories = JsonConvert.DeserializeObject(objReader.ReadToEnd());
            List<Story> stories = new List<Story>();
            //parse each story
            foreach (dynamic d in dynStories)
            {
                dynamic dd = d.categories;
                List<String> categories = new List<string>();
                foreach (String ddd in dd)
                {
                    categories.Add(ddd);
                }
                //add story to list
                stories.Add(new Story(System.Convert.ToString(d.title), System.Convert.ToString(d.description), System.Convert.ToString(d.uri), categories, System.Convert.ToString(d.imgUri)));

            }
            //return all stories
            return stories;

        }
        public static void addLike(String like, String id)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Story));
            //add like to user's account
            String url = App.apiURL + "addLike/" + id + "/" + like;
            WebRequest wrGETURL = WebRequest.Create(url);
            wrGETURL.Proxy = null;

            wrGETURL.GetResponseAsync();
        }
    }
}
