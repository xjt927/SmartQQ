﻿using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartQQ.Core.Client;
using SmartQQ.Core.Constants;
using SmartQQ.Core.Utils;

namespace SmartQQ.Core.Models
{
    /// <summary>
    ///     好友分组。
    /// </summary>
    public class FriendCategory : IListable
    {
        [JsonIgnore] internal SmartQQClient Client;

        /// <summary>
        ///     序号。
        /// </summary>
        [JsonProperty("index")]
        public int Index { get; set; }

        /// <summary>
        ///     名称。
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///     成员。
        /// </summary>
        [JsonIgnore]
        public List<Friend> Members => _members.GetValue(() => Client.Friends.FindAll(_ => _.CategoryIndex == Index));

        [JsonIgnore] private readonly LazyHelper<List<Friend>> _members = new LazyHelper<List<Friend>>();

        public int OnlineFriendNum => Members.Count;

        public int TotalFriendNum => Members.Count;

        /// <summary>
        ///     用于初始化默认分组。
        /// </summary>
        public static FriendCategory DefaultCategory()
        {
            return new FriendCategory
            {
                Index = 0,
                Name = "我的好友"
            };
        }

        internal static List<FriendCategory> GetList(SmartQQClient client)
        {
            SmartQQClient.Logger.Debug("开始获取好友列表");
            var response = client.Client.Post(ApiUrl.GetFriendList,
                new JObject {{"vfwebqq", client.Vfwebqq}, {"hash", client.Hash}});
            var result = (JObject) client.GetResponseJson(response)["result"];
            //获得分组
            var categories = result["categories"] as JArray;
            //var categoryDictionary = new Dictionary<int, FriendCategory> { { 0, DefaultCategory() } };
            var categoryDictionary = new Dictionary<int, FriendCategory> {};
            for (var i = 0; categories != null && i < categories.Count; i++)
            {
                var category = categories[i].ToObject<FriendCategory>();
                categoryDictionary.Add(category.Index, category);
            }
            foreach (var category in categoryDictionary.Values)
                category.Client = client;
            return categoryDictionary.Select(_ => _.Value).ToList();
        }

        /// <inheritdoc />
        protected bool Equals(FriendCategory other)
        {
            return base.Equals(other) && Index == other.Index;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Friend) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() => Index.GetHashCode();

        /// <inheritdoc />
        public static bool operator ==(FriendCategory left, FriendCategory right) => left?.Index == right?.Index;

        /// <inheritdoc />
        public static bool operator !=(FriendCategory left, FriendCategory right) => !(left == right);
    }
}