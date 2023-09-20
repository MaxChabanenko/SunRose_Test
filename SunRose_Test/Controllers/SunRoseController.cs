using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SunRose_Test.Models;
using SunRose_Test.Repository;
using System.Dynamic;

namespace SunRose_Test.Controllers
{
    public class SunRoseController : Controller
    {
        private List<User> usersList;
        private readonly JsonRepository<User> JsonRepository;
        private readonly int oneUserMaxMessages=10;
        public SunRoseController()
        {
            usersList = new List<User>();
            JsonRepository= new JsonRepository<User>();
            //messageIdCounter = 1;
        }
        [HttpGet]
        [Route("SunRose/Post/{userId:long}")]
        public ActionResult Post(long userId)
        {
            usersList = JsonRepository.Read();

            User user = usersList.Where(s => s.Id == userId).FirstOrDefault();

            Message newPost = new Message() {  UserId = user.Id };

            UserAndMessage mymodel = new UserAndMessage();
            mymodel.user = user;
            mymodel.message = newPost;
            return View(mymodel);
        }

        [HttpPost]
        [Route("SunRose/Post/{userId:long}")]
        public ActionResult Post(UserAndMessage mymodel)
        {
            usersList = JsonRepository.Read();
            Message msg = mymodel.message;

            var user = usersList.Where(x => x.Id == msg.UserId).FirstOrDefault();
            usersList.Remove(user);
            //msg.Id = messageIdCounter++;
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

            Message newPost = new Message() {  UserId = user.Id };

            mymodel.user = user;
            mymodel.message = newPost;

            return View(mymodel);
        }

        public ActionResult Index()
        {
            usersList = JsonRepository.Read();
            return View(usersList);
        }

        public ActionResult SignUp()
        {
            User user = new User();

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

                if(user==null)
                    usersList.Add(newUser);
                else
                {
                    ModelState.AddModelError("UserIdExist", "User with this Id already exists");
                    return View();
                }

                JsonRepository.Create(usersList);

                return RedirectToAction("Index");
            }
            else
                return View();
            
        }
       

    }
}
