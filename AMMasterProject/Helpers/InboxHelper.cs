using AMMasterProject.Models;
using AMMasterProject.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject.Helpers
{
    public class InboxHelper
    {

        private readonly MyDbContext _dbContext;

        public InboxHelper(MyDbContext context)
        {
            _dbContext = context;

        }


        #region ContactList

        public async Task<List<InboxViewModel>> inboxmycontacts(int profileid)
        {
            var receiver = await (from m in _dbContext.MessageMaser
                                  join u in _dbContext.UsersProfiles on m.senderid equals u.ProfileId
                                  where m.senderid == profileid
                                  select new InboxViewModel
                                  {
                                      chatid = m.ChatId.ToString(),
                                      contactid = u.ProfileId,
                                      contactguid=u.ProfileGuid,
                                      fullname = u.Firstname + " " + u.Lastname,
                                      image = u.ProfileImage,
                                      firstchar = u.Firstname.Substring(0, 1).ToLower(),
                                      type = "sender",
                                      message = (from msg in _dbContext.Messages
                                                 where msg.ChatId == m.ChatId 
                                                 orderby msg.MesageId descending
                                                 select msg.Message1).FirstOrDefault(),

                                      messageid = (from msg in _dbContext.Messages
                                                    where msg.ChatId == m.ChatId
                                                    orderby msg.MesageId descending
                                                    select msg.MesageId).FirstOrDefault(),


                                      readstatus = (from msg in _dbContext.Messages
                                                    where msg.ChatId == m.ChatId && msg.Receiverid == profileid
                                                    orderby msg.MesageId descending
                                                    select msg.Status).FirstOrDefault()
                                  }).ToListAsync();

            var sender = await (from m in _dbContext.MessageMaser
                                join u in _dbContext.UsersProfiles on m.receiverid equals u.ProfileId
                                where m.receiverid == profileid
                                select new InboxViewModel
                                {
                                    chatid = m.ChatId.ToString(),
                                    contactid = u.ProfileId,
                                    contactguid = u.ProfileGuid,
                                    fullname = u.Firstname + " " + u.Lastname,
                                    image = u.SellerImage,
                                    firstchar = u.Firstname.Substring(0, 1).ToLower(),
                                    type = "receive",
                                    message = (from msg in _dbContext.Messages
                                               where msg.ChatId == m.ChatId
                                               orderby msg.MesageId descending
                                               select msg.Message1).FirstOrDefault(),

                                    messageid = (from msg in _dbContext.Messages
                                                 where msg.ChatId == m.ChatId
                                                 orderby msg.MesageId descending
                                                 select msg.MesageId).FirstOrDefault(),
                                    readstatus = (from msg in _dbContext.Messages
                                                  where msg.ChatId == m.ChatId && msg.Receiverid  ==profileid
                                                  orderby msg.MesageId descending
                                                  select msg.Status).FirstOrDefault()
                                }).ToListAsync();

            var q = receiver.Union(sender).Distinct().ToList();


            return q;
        }
        #endregion


        #region CreateChat


        public string createchat(int senderid, int receiverid)
        {

            string chatid = string.Empty;

            MessageMaster sender = _dbContext.MessageMaser.FirstOrDefault(u => u.senderid == senderid && u.receiverid == receiverid);

            if (sender == null)
            {
                MessageMaster receiver = _dbContext.MessageMaser.FirstOrDefault(u => u.receiverid == senderid && u.senderid == receiverid);

                if (receiver == null)
                {

                    ///create new chat id
                    ///

                    MessageMaster mm = new MessageMaster();

                    mm.senderid = senderid;
                    mm.receiverid = receiverid;


                    _dbContext.MessageMaser.Add(mm);
                    _dbContext.SaveChanges();

                    if (mm != null)
                    {
                        chatid = mm.ChatId.ToString();

                        string currenturl = "";
                        Message m = new Message();
                        m.ChatId = Guid.Parse(chatid.ToString());
                        m.Senderid = senderid;
                        m.Receiverid = receiverid;
                        m.Message1 = "I want to know more about you " + currenturl;
                        m.Senddate = DateTime.Now;
                        m.Status = "UnRead";
                        _dbContext.Messages.Add(m);
                        _dbContext.SaveChanges();
                    }


                }
                else
                {
                    chatid = receiver.ChatId.ToString();
                }

            }
            else
            {

                chatid = sender.ChatId.ToString();
            }




            return chatid;
        }





        #endregion

        #region MessageList
        public List<InboxViewModel> messagelist(Guid chatid, int loginuserid)
        {
            List<InboxViewModel> list = new List<InboxViewModel>();

            var sender = (from m in _dbContext.Messages
                          join u in _dbContext.UsersProfiles on m.Senderid equals u.ProfileId
                          where m.ChatId == chatid && m.Senderid == loginuserid
                          select new InboxViewModel
                          {
                              messageid = m.MesageId,
                              chatid = m.ChatId.ToString(),
                              contactid = u.ProfileId,
                              contactguid = u.ProfileGuid,
                              fullname = u.Firstname + " " + u.Lastname,
                              image = u.ProfileImage,
                              firstchar = u.Firstname.Substring(0, 1).ToLower(),
                              message = m.Message1,
                              insertdate = m.Senddate.ToString(),
                              attachment = m.Attachment,
                              filename=m.FileName,
                              type = "send",
                              readstatus=m.Status
                          });


         

            int scount = sender.Count();

            var receiver = (from m in _dbContext.Messages
                            join u in _dbContext.UsersProfiles on m.Senderid equals u.ProfileId
                            where m.ChatId == chatid && m.Receiverid == loginuserid
                            select new InboxViewModel
                            {
                                messageid = m.MesageId,
                                chatid = m.ChatId.ToString(),
                                contactid = u.ProfileId,
                                contactguid = u.ProfileGuid,
                                fullname = u.Firstname + " " + u.Lastname,
                                image = u.ProfileImage,
                                firstchar = u.Firstname.Substring(0, 1).ToLower(),
                                message = m.Message1,
                                insertdate = m.Senddate.ToString(),
                                attachment = m.Attachment,
                                filename = m.FileName,
                                type = "receiver",
                                readstatus = m.Status
                            });

         

            ///make bulk update if any status is UnRead so make it Reads
            int rcount = receiver.Count();
            var messages = receiver.Union(sender)

                .OrderByDescending(m => m.messageid).Distinct()
                //.Skip(skip)
                //.Take(take)
                .ToList();



            list = messages;

            return list;
        }
        #endregion

        #region SendMessage

        public string insertmessage(Guid chatid,string message, string attachment, string filename, int loginuserid)
        {
            string prompt = string.Empty;

            try
            {



                int senderid = loginuserid;
                int receiverid = 0;

                //first determine receier id
                MessageMaster master = _dbContext.MessageMaser.FirstOrDefault(u => u.ChatId == chatid);

                if (master != null)
                {

                    if (master.senderid == loginuserid)
                    {
                        receiverid = master.receiverid;
                    }
                    else
                    {
                        receiverid = master.senderid;
                    }
                }

                Message mi = new Message();
                mi.ChatId = chatid;
                mi.Senderid = senderid;
                mi.Receiverid = receiverid;
                mi.Senddate = DateTime.Now;
                mi.Status = "UnRead";
                mi.Message1 = message;
                if (attachment !=null)
                {
                    mi.Attachment = attachment;
                    mi.FileName = filename;
                }
                _dbContext.Messages.Add(mi);
                _dbContext.SaveChanges();

                prompt = "success";
            }
            catch (Exception ex)
            {

                prompt = "Error: " + ex.Message;
            }

            return prompt;
        }

        #endregion

        #region MyRegion
        public async Task<AnnouncementCounterViewModel> InboxUnreadCount(int profileid)
        {
            int unreadCount = (await inboxmycontacts(profileid))
                .Count(u => u.readstatus == "UnRead");

            return new AnnouncementCounterViewModel
            {
                AnnouncementCounter = unreadCount
            };
        }
        #endregion
    }
}
