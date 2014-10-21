using Processo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Areas.WebService.Controllers
{
    public class BaseApiController : ApiController
    {
        protected UnityOfWork unityOfWork;

        public BaseApiController()
        {
            unityOfWork = UnityOfWork.GetInstancia();
        }
    }
}
