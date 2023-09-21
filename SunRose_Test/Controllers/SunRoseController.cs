using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SunRose_Test.Models;
using SunRose_Test.Models.ModelViews;
using SunRose_Test.Repository;
using System.Dynamic;

namespace SunRose_Test.Controllers
{
    public class SunRoseController : Controller
    {
        private List<User> usersList;
        private readonly JsonRepository<User> JsonRepository;
        private readonly int oneUserMaxMessages = 10;
        private readonly int feedMaxMessages = 20;
        static private int nextId;

        public SunRoseController()
        {
            usersList = new List<User>();
            JsonRepository = new JsonRepository<User>();
        }
        [HttpGet]
        public ActionResult Feed(SortingOption sortBy = SortingOption.Id)
        {
            usersList = JsonRepository.Read();
            List<Message> allMessages = new List<Message>();

            foreach (User user in usersList)
                allMessages.AddRange(user.Feed);

            allMessages = allMessages.OrderBy(m => m.CreatedDate).Reverse().Take(feedMaxMessages).ToList();

            if (sortBy == SortingOption.Date)
            {
                allMessages = allMessages.OrderBy(m => m.CreatedDate).Reverse().ToList();
            }
            else
            {
                allMessages = allMessages.OrderBy(m => m.UserId).ToList();
            }

            var viewModel = new MessagesAndSort
            {
                Messages = allMessages,
                SortBy = sortBy
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Post(int userId)
        {
            usersList = JsonRepository.Read();

            User user = usersList.Where(s => s.Id == userId).FirstOrDefault();

            Message newPost = new Message() { UserId = user.Id };

            UserAndMessage mymodel = new UserAndMessage();
            mymodel.User = user;
            mymodel.Message = newPost;
            return View(mymodel);
        }

        [HttpPost]
        public ActionResult Post(UserAndMessage mymodel)
        {
            usersList = JsonRepository.Read();
            Message msg = mymodel.Message;

            var user = usersList.Where(x => x.Id == msg.UserId).FirstOrDefault();
            usersList.Remove(user);
            if (user.Feed.Count() + 1 > oneUserMaxMessages)
            {
                user.Feed.Dequeue();
                user.Feed.Enqueue(msg);
            }
            else
            {
                user.Feed.Enqueue(msg);
            }
            usersList.Add(user);

            JsonRepository.Create(usersList);

            Message newPost = new Message() { UserId = user.Id };

            mymodel.User = user;
            mymodel.Message = newPost;

            return View(mymodel);
        }

        public ActionResult Index()
        {
            usersList = JsonRepository.Read();
            return View(usersList);
        }

        public ActionResult SignUp()
        {
            //tried using ToUnixTimeMilliseconds() at least it would give unique ids, but it was over the top compared to just increment
            //with guid user couldn't make his own id
            User user = new User(nextId, "");
            return View(user);
        }

        [HttpPost]
        public ActionResult SignUp(User newUser)
        {
            if (ModelState.IsValid)
            {
                //through many searches and forums I never found how to avoid .net binding empty collection as null, so this workaround
                newUser.Feed = new Queue<Message>();

                usersList = JsonRepository.Read();

                var user = usersList.Where(x => x.Id == newUser.Id).FirstOrDefault();

                if (user == null)
                    usersList.Add(newUser);
                else
                {
                    ModelState.AddModelError("UserIdExist", "User with this Id already exists");
                    return View();
                }

                //increment userId only after creating one
                nextId++;

                JsonRepository.Create(usersList);

                return RedirectToAction("Index");
            }
            else
                return View();

        }
    }
}
