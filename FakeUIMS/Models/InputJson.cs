#pragma warning disable IDE1006

using System;

namespace FakeUIMS.Models
{
    public class InputBase
    {
        public string tag { get; set; }
        public string res { get; set; }
        public string branch { get; set; }
        public PARAMS @params { get; set; }

        public class PARAMS
        {
            public int termId { get; set; }
        }
    }

    class ArchiveScoreValue
    {
        public string xkkh { get; set; }
        public TeachingTerm teachingTerm { get; set; }
        public string score { get; set; }
        public DateTime dateScore { get; set; }
        public string isPass { get; set; }
        public Course course { get; set; }
        public string isReselect { get; set; }
        public string gpoint { get; set; }
        public string credit { get; set; }

        public class Course
        {
            public string courName { get; set; }
        }

        public class TeachingTerm
        {
            public string termName { get; set; }
        }
    }

    class RootObject<T>
    {
        public string id { get; set; }
        public int status { get; set; }
        public T[] value { get; set; }
        public string resName { get; set; }
        public string msg { get; set; }
        public string data { get; set; }
    }

    class TeachingTerm
    {
        public TeachingTerm() { }

        public static string GetRootObject()
        {
            return "{\"id\":\"termId\",\"status\":0,\"value\":[" +
                "{\"termName\":\"2018-2019第2学期\",\"startDate\":\"2019-03-04T00:00:00\",\"termSeq\":\"2\",\"examDate\":\"2019-07-01T00:00:00\",\"activeStage\":\"1200\",\"year\":\"2018-2019\",\"vacationDate\":\"2019-07-08T00:00:00\",\"weeks\":\"18\",\"termId\":\"136\",\"egrade\":\"2018\"}," +
                "{\"termName\":\"2018-2019第1学期\",\"startDate\":\"2018-08-27T00:00:00\",\"termSeq\":\"1\",\"examDate\":\"2019-01-07T00:00:00\",\"activeStage\":\"1230\",\"year\":\"2018-2019\",\"vacationDate\":\"2019-01-21T00:00:00\",\"weeks\":\"21\",\"termId\":\"135\",\"egrade\":\"2018\"}," +
                "{\"termName\":\"2017-2018第2学期\",\"startDate\":\"2018-03-05T00:00:00\",\"termSeq\":\"2\",\"examDate\":\"2018-07-02T00:00:00\",\"activeStage\":\"1220\",\"year\":\"2017-2018\",\"vacationDate\":\"2018-07-09T00:00:00\",\"weeks\":\"18\",\"termId\":\"134\",\"egrade\":\"2017\"}," +
                "{\"termName\":\"2017-2018第1学期\",\"startDate\":\"2017-08-28T00:00:00\",\"termSeq\":\"1\",\"examDate\":\"2018-01-08T00:00:00\",\"activeStage\":\"1230\",\"year\":\"2017-2018\",\"vacationDate\":\"2018-01-22T00:00:00\",\"weeks\":\"21\",\"termId\":\"133\",\"egrade\":\"2017\"}," +
                "{\"termName\":\"2016-2017第2学期\",\"startDate\":\"2017-02-27T00:00:00\",\"termSeq\":\"2\",\"examDate\":\"2017-06-26T00:00:00\",\"activeStage\":\"1230\",\"year\":\"2016-2017\",\"vacationDate\":\"2017-07-10T00:00:00\",\"weeks\":\"19\",\"termId\":\"132\",\"egrade\":\"2016\"}," +
                "{\"termName\":\"2016-2017第1学期\",\"startDate\":\"2016-08-22T00:00:00\",\"termSeq\":\"1\",\"examDate\":\"2017-01-02T00:00:00\",\"activeStage\":\"1230\",\"year\":\"2016-2017\",\"vacationDate\":\"2017-01-16T00:00:00\",\"weeks\":\"21\",\"termId\":\"131\",\"egrade\":\"2016\"}," +
                "{\"termName\":\"2015-2016第2学期\",\"startDate\":\"2016-03-07T00:00:00\",\"termSeq\":\"2\",\"examDate\":\"2016-07-04T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2015-2016\",\"vacationDate\":\"2016-07-11T00:00:00\",\"weeks\":\"18\",\"termId\":\"130\",\"egrade\":\"2015\"}," +
                "{\"termName\":\"2015-2016第1学期\",\"startDate\":\"2015-08-24T00:00:00\",\"termSeq\":\"1\",\"examDate\":\"2016-01-04T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2015-2016\",\"vacationDate\":\"2016-01-18T00:00:00\",\"weeks\":\"21\",\"termId\":\"129\",\"egrade\":\"2015\"}," +
                "{\"termName\":\"2014-2015第2学期\",\"startDate\":\"2015-03-09T00:00:00\",\"termSeq\":\"2\",\"examDate\":\"2015-06-30T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2014-2015\",\"vacationDate\":\"2015-07-12T00:00:00\",\"weeks\":\"19\",\"termId\":\"128\",\"egrade\":\"2014\"}," +
                "{\"termName\":\"2014-2015第1学期\",\"startDate\":\"2014-09-01T00:00:00\",\"termSeq\":\"1\",\"examDate\":\"2015-01-05T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2014-2015\",\"vacationDate\":\"2015-01-19T00:00:00\",\"weeks\":\"20\",\"termId\":\"127\",\"egrade\":\"2014\"}," +
                "{\"termName\":\"2013-2014第2学期\",\"startDate\":\"2014-03-03T00:00:00\",\"termSeq\":\"2\",\"examDate\":\"2014-06-30T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2013-2014\",\"vacationDate\":\"2014-07-14T00:00:00\",\"weeks\":\"19\",\"termId\":\"126\",\"egrade\":\"2013\"}," +
                "{\"termName\":\"2013-2014第1学期\",\"startDate\":\"2013-09-16T00:00:00\",\"termSeq\":\"1\",\"examDate\":\"2014-01-06T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2013-2014\",\"vacationDate\":\"2014-01-13T00:00:00\",\"weeks\":\"17\",\"termId\":\"125\",\"egrade\":\"2013\"}," +
                "{\"termName\":\"2012-2013第3学期\",\"startDate\":\"2013-08-12T00:00:00\",\"termSeq\":\"3\",\"examDate\":\"1999-06-21T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2012-2013\",\"vacationDate\":\"2013-09-16T00:00:00\",\"weeks\":\"5\",\"termId\":\"124\",\"egrade\":\"2012\"}," +
                "{\"termName\":\"2012-2013第2学期\",\"startDate\":\"2013-03-03T00:00:00\",\"termSeq\":\"2\",\"examDate\":\"2013-06-21T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2012-2013\",\"vacationDate\":\"2013-07-01T00:00:00\",\"weeks\":\"16\",\"termId\":\"123\",\"egrade\":\"2012\"}," +
                "{\"termName\":\"2012-2013第1学期\",\"startDate\":\"2012-09-17T00:00:00\",\"termSeq\":\"1\",\"examDate\":\"2013-01-07T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2012-2013\",\"vacationDate\":\"2013-01-13T00:00:00\",\"weeks\":\"17\",\"termId\":\"122\",\"egrade\":\"2012\"}," +
                "{\"termName\":\"2011-2012第3学期\",\"startDate\":\"2012-08-13T00:00:00\",\"termSeq\":\"3\",\"examDate\":\"2012-09-10T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2011-2012\",\"vacationDate\":\"2012-09-16T00:00:00\",\"weeks\":\"5\",\"termId\":\"121\",\"egrade\":\"2011\"}," +
                "{\"termName\":\"2011-2012第2学期\",\"startDate\":\"2011-09-19T00:00:00\",\"termSeq\":\"2\",\"examDate\":\"2012-01-16T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2011-2012\",\"vacationDate\":\"2012-01-16T00:00:00\",\"weeks\":\"17\",\"termId\":\"120\",\"egrade\":\"2011\"}," +
                "{\"termName\":\"2011-2012第1学期\",\"startDate\":\"2011-09-19T00:00:00\",\"termSeq\":\"1\",\"examDate\":\"2012-01-16T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2011-2012\",\"vacationDate\":\"2012-01-16T00:00:00\",\"weeks\":\"17\",\"termId\":\"119\",\"egrade\":\"2011\"}," +
                "{\"termName\":\"2010-2011第3学期\",\"startDate\":\"2011-08-15T00:00:00\",\"termSeq\":\"3\",\"examDate\":\"1999-06-21T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2010-2011\",\"vacationDate\":\"2011-09-19T00:00:00\",\"weeks\":\"6\",\"termId\":\"118\",\"egrade\":\"2010\"}," +
                "{\"termName\":\"2010-2011第2学期\",\"startDate\":\"2011-03-07T00:00:00\",\"termSeq\":\"2\",\"examDate\":\"2010-12-07T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2010-2011\",\"vacationDate\":\"2011-07-04T00:00:00\",\"weeks\":\"16\",\"termId\":\"117\",\"egrade\":\"2010\"}," +
                "{\"termName\":\"2010-2011第1学期\",\"startDate\":\"2010-09-20T00:00:00\",\"termSeq\":\"1\",\"examDate\":\"2011-01-10T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2010-2011\",\"vacationDate\":\"2011-01-17T00:00:00\",\"weeks\":\"16\",\"termId\":\"116\",\"egrade\":\"2010\"}," +
                "{\"termName\":\"2009-2010第3学期\",\"startDate\":\"2010-08-16T00:00:00\",\"termSeq\":\"3\",\"examDate\":\"1999-06-21T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2009-2010\",\"vacationDate\":\"2010-09-20T00:00:00\",\"weeks\":\"6\",\"termId\":\"115\",\"egrade\":\"2009\"}," +
                "{\"termName\":\"2009-2010第2学期\",\"startDate\":\"2010-03-08T00:00:00\",\"termSeq\":\"2\",\"examDate\":\"1999-06-21T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2009-2010\",\"vacationDate\":\"2010-07-05T00:00:00\",\"weeks\":\"16\",\"termId\":\"114\",\"egrade\":\"2009\"}," +
                "{\"termName\":\"2009-2010第1学期\",\"startDate\":\"2009-09-21T00:00:00\",\"termSeq\":\"1\",\"examDate\":\"1999-06-21T00:00:00\",\"activeStage\":\"1235\",\"year\":\"2009-2010\",\"vacationDate\":\"2010-01-25T00:00:00\",\"weeks\":\"16\",\"termId\":\"113\",\"egrade\":\"2009\"}" +
                "],\"resName\":\"teachingTerm\",\"msg\":\"\"}";
        }
        
        public string termName { get; set; }
        public DateTime startDate { get; set; }
        public string termSeq { get; set; }
        public DateTime examDate { get; set; }
        public string activeStage { get; set; }
        public string year { get; set; }
        public DateTime vacationDate { get; set; }
        public string weeks { get; set; }
        public string termId { get; set; }
        public string egrade { get; set; }
    }

    class MessageBox
    {
        public MessageBox() { }

        public MessageBox(bool f)
        {
            count = Program.Random.Next(0, 10);
            items = new MessagePiece[count];
            for (int i = 0; i < count; i++)
                items[i] = new MessagePiece(f);
        }

        public int count { get; set; }
        public int errno { get; set; } = 0;
        public string identifier { get; set; } = "msgInboxId";
        public MessagePiece[] items { get; set; }
        public string label { get; set; } = "";
        public string msg { get; set; } = "";
        public int status { get; set; } = 0;
    }

    class MessagePiece
    {
        public MessagePiece() { }

        public MessagePiece(bool f)
        {
            message = new MessageMain(f);
            receiver = new MessageReceiver(f);
            msgInboxId = Program.Random.Next(9600000, 9876543).ToString();
            hasReaded = Program.Random.Next(0, 10) > 5 ? "N" : "Y";
            activeStatus = "103";
        }

        public MessageMain message { get; set; }
        public string msgInboxId { get; set; }
        public MessageReceiver receiver { get; set; }
        public object dateRead { get; set; }
        public string activeStatus { get; set; }
        public string hasReaded { get; set; }
        public object dateReceive { get; set; }

        public class MessageReceiver
        {
            public MessageReceiver() { }

            public MessageReceiver(bool f)
            {
                school = new MessageSchool(f);
                name = "测试账户";
            }

            public MessageSchool school { get; set; }
            public string name { get; set; }

            public class MessageSchool
            {
                public MessageSchool() { }

                public MessageSchool(bool f)
                {
                    schoolName = "软件学院";
                }

                public string schoolName { get; set; }
            }
        }

        public class MessageMain
        {
            public MessageMain() { }

            public MessageMain(bool f)
            {
                if (Program.Random.Next(0, 100) > 50)
                    sender = new MessageSender { name = Helper.GetRandomChinese(3) };
                body = Helper.GetRandomChinese(Program.Random.Next(10, 100));
                title = Helper.GetRandomChinese(Program.Random.Next(10, 20));
                dateCreate = Helper.GetRandomTime().AddDays(-Program.Random.Next(2, 5));
                dateExpire = Helper.GetRandomTime().AddDays(Program.Random.Next(2, 5));
                messageId = Program.Random.Next(254719, 584719).ToString();
            }

            public MessageSender sender { get; set; }
            public string body { get; set; }
            public string title { get; set; }
            public DateTime dateExpire { get; set; }
            public string messageId { get; set; }
            public DateTime dateCreate { get; set; }

            public class MessageSender
            {
                public string name { get; set; }
            }
        }
    }

    class LoginValue
    {
        public LoginValue() { }

        public LoginValue(string alu)
        {
            loginMethod = "SIMPLE";

            cacheUpdate = new CacheUpdate
            {
                sysDict = 1357028821921,
                code = 1535956863000
            };

            menusFile = new[]
            {
                "STUDENT.json",
                "everyone.json"
            };

            trulySch = 101;
            groupsInfo = new GroupsInfo[0];

            defRes = new DefRes
            {
                adcId = 7097,
                term_l = 136,
                teachingTerm = 135,
                school = 101,
                department = 632,
                term_a = 136,
                schType = 1450,
                personId = 666666,
                year = 2018,
                term_s = 135,
                campus = 1401,
            };

            userType = "S";
            sysTime = DateTime.Now;
            nickName = "测试账户";
            userId = 666666;
            welcome = "welcome";
            loginName = alu;
        }

        public string loginMethod { get; set; }
        public CacheUpdate cacheUpdate { get; set; }
        public string[] menusFile { get; set; }
        public int trulySch { get; set; }
        public GroupsInfo[] groupsInfo { get; set; }
        public string firstLogin { get; set; }
        public DefRes defRes { get; set; }
        public string userType { get; set; }
        public DateTime sysTime { get; set; }
        public string nickName { get; set; }
        public int userId { get; set; }
        public string welcome { get; set; }
        public string loginName { get; set; }

        public class CacheUpdate
        {
            public long sysDict { get; set; }
            public long code { get; set; }
        }

        public class DefRes
        {
            public int adcId { get; set; }
            public int term_l { get; set; }
            public int university { get; set; }
            public int teachingTerm { get; set; }
            public int school { get; set; }
            public int department { get; set; }
            public int term_a { get; set; }
            public int schType { get; set; }
            public int personId { get; set; }
            public int year { get; set; }
            public int term_s { get; set; }
            public int campus { get; set; }
        }

        public class GroupsInfo
        {
            public int groupId { get; set; }
            public string groupName { get; set; }
            public string menuFile { get; set; }
        }
    }
    
    class GradeDetails
    {
        public GradeDetails() { }

        public GradeDetails(bool f)
        {
            count = 5;
            errno = 0;
            identifier = "seq";
            label = "label";
            msg = "";
            status = 0;

            int pool = 100;
            items = new GradeEntry[5];

            var labels = new[]
            {
                "优秀(>90)",
                "良好(80-90)",
                "中等(70-80)",
                "及格(60-70)",
                "不及格(<60)",
            };
            
            for (int seq = 0; seq < 5; seq++)
            {
                int cnt = Program.Random.Next(0, pool);
                pool -= cnt;

                items[seq] = new GradeEntry
                {
                    count = 0,
                    label = labels[seq],
                    percent = cnt,
                    seq = seq,
                };
            }
        }

        public int count { get; set; }
        public int errno { get; set; }
        public string identifier { get; set; }
        public GradeEntry[] items { get; set; }
        public string label { get; set; }
        public string msg { get; set; }
        public int status { get; set; }

        public class GradeEntry
        {
            public int count { get; set; }
            public string label { get; set; }
            public float percent { get; set; }
            public int seq { get; set; }
        }
    }
}

#pragma warning restore
