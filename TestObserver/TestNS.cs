using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;

namespace TestObserver
{
    [TestClass]
    public class TestNS
    {
        [DataTestMethod]
        [DataRow("devs", "personal", "personal.devs")]
        [DataRow("funny", "personal", "funny")]
        [DataRow("devs", "work", "work.devs")]
        [DataRow("issues", "work", "issues")]
        public void Test1(string chName, string nsName, string expected)
        {

            var slack = new Slack()
            {
                Namespaces = new List<Workspace>()
                {
                    new Workspace()
                    {
                        Name = "personal",
                        Channels = new List<Channel>()
                        {
                            new Channel()
                            {
                                Name = "devs",
                            },
                            new Channel()
                            {
                                Name = "funny",
                            },
                        }
                    },
                    new Workspace()
                    {
                        Name = "work",
                        Channels = new List<Channel>()
                        {
                            new Channel()
                            {
                                Name = "devs",
                            },
                            new Channel()
                            {
                                Name = "issues",
                            },
                        }
                    }
                }
            };

            var ns = slack.Namespaces.Where(x => x.Name == nsName).Single();
            var ch = ns.Channels.Where(x => x.Name == chName).Single();
            Assert.AreEqual(expected, slack.DisplayName(ns, ch));
            

        }
    }


    public class Slack
    {
        public List<Workspace> Namespaces { get; set; }
        public string DisplayName(Workspace workspace, Channel channel) => Namespaces.SelectMany(x => x.Channels, (ws, ch) => new { ws, ch }).Where(x => x.ch.Name == channel.Name).Count() > 1 ? $"{workspace.Name}.{channel.Name}" : channel.Name;
    }

    public class Workspace
    {
        public string Name { get; set; }
        public List<Channel> Channels { get; set; }
    }

    public class Channel
    {
        public string Name { get; set; }
    }
}