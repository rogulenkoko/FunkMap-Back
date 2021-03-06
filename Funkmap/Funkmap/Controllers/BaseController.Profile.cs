﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using Funkmap.Common.Models;
using Funkmap.Common.Owin.Auth;
using Funkmap.Common.Owin.Extensions;
using Funkmap.Domain.Abstract;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Mappers;
using Funkmap.Models;
using Funkmap.Models.Requests;
using Funkmap.Tools;

namespace Funkmap.Controllers
{
    public partial class BaseController
    {
        /// <summary>
        /// Get full profile information.
        /// </summary>
        /// <param name="login">Profile login</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Profile))]
        [Route("profile/{login}")]
        public async Task<IHttpActionResult> GetFull(string login)
        {
            var entity = await _queryRepository.GetAsync(login);
            if (entity == null)
            {
                return BadRequest("Invalid profile login.");
            }
            Request.SetProfileCorrectAvatarUrls(entity);
            return Content(HttpStatusCode.OK, entity);
        }
        

        /// <summary>
        /// Create a profile.
        /// </summary>
        /// <param name="model">Profile</param>
        [Authorize]
        [HttpPost]
        [ResponseType(typeof(BaseResponse))]
        [Route("profile")]
        public async Task<IHttpActionResult> Create([ModelBinder(typeof(FunkmapModelBinderProvider))]Profile model)
        {
            var userLogin = Request.GetLogin();
            var canCreateResult = await _accessService.CanCreateProfileAsync(userLogin);

            if (!canCreateResult.CanCreate)
            {
                return BadRequest(canCreateResult.Reason);
            }

            var result = await _commandRepository.CreateAsync(new CommandParameter<Profile> { Parameter = model, UserLogin = userLogin });
            return Ok(new BaseResponse() { Success = result.Success, Error = result.Error });

        }

        /// <summary>
        /// Check can user add new profiles
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("profile/can-create")]
        public async Task<IHttpActionResult> CanCreate()
        {
            var userLogin = Request.GetLogin();
            var canCreateResult = await _accessService.CanCreateProfileAsync(userLogin);
            return Ok(canCreateResult);
        }


        /// <summary>
        /// Update a profile.
        /// You can't modify EntityType, Login, UserLogin, AvatarId. For avatar updating use "base/avatar" POST method.
        /// You can't modify band participants and musician's band. You should use specific API methods.
        /// </summary>
        /// <param name="model">Profile model which has only updated properties.</param>
        [HttpPut]
        [Route("profile")]
        [ResponseType(typeof(BaseResponse))]
        [Authorize]
        public async Task<IHttpActionResult> Update([ModelBinder(typeof(FunkmapModelBinderProvider))]Profile model)
        {
            var login = Request.GetLogin();

            var parameter = new CommandParameter<Profile>
            {
                UserLogin = login,
                Parameter = model
            };

            var updateResult = await _commandRepository.UpdateAsync(parameter);
            return Ok(new BaseResponse { Success = updateResult.Success, Error = updateResult.Error });
        }

        /// <summary>
        /// Delete profile.
        /// </summary>
        /// <param name="login">Profile login</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("profile/{login}")]
        [Authorize]
        public async Task<IHttpActionResult> Delete(string login)
        {
            var userLogin = Request.GetLogin();

            var parameter = new CommandParameter<string>() { UserLogin = userLogin, Parameter = login };
            var deleteResult = await _commandRepository.DeleteAsync(parameter);

            return Ok(new BaseResponse() { Success = deleteResult.Success, Error = deleteResult.Error });
        }

        /// <summary>
        /// Profile's base information (specific for each profile type).
        /// </summary>
        /// <param name="login">Profile's login</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ProfilePreview))]
        [Route("profile-preview/{login}")]
        public async Task<IHttpActionResult> Get(string login)
        {
            var entity = await _queryRepository.GetAsync(login);
            if (entity == null)
            {
                return BadRequest("Invalid profile login.");
            }
            Request.SetProfileCorrectAvatarUrls(entity);
            return Content(HttpStatusCode.OK, entity.ToSpecificPreviewModel());
        }

        /// <summary>
        /// Get profile avatar.
        /// (Byte array or base64 string)
        /// </summary>
        /// <param name="login">Profile's login</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(byte[]))]
        [AllowAnonymous]
        [Route("avatar/{login}")]
        public async Task<HttpResponseMessage> GetImage(string login)
        {
            byte[] file = await _queryRepository.GetFileAsync(login);
            return ControllerMedia.MediaResponse(file);
        }

        /// <summary>
        /// Update profile's avatar.
        /// (Put null or empty byte array for avatar deleting)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("avatar")]
        [ResponseType(typeof(BaseResponse))]
        [Authorize]
        public async Task<IHttpActionResult> UpdateAvatar(UpdateAvatarRequest request)
        {
            var login = Request.GetLogin();

            var commandParameter = new CommandParameter<AvatarUpdateParameter>()
            {
                UserLogin = login,
                Parameter = new AvatarUpdateParameter() { Login = request.Login, Bytes = request.Photo }
            };

            ICommandResponse result = await _commandRepository.UpdateAvatarAsync(commandParameter);

            return Ok(new BaseResponse() { Success = result.Success, Error = result.Error });
        }


        /// <summary>
        /// Profile's logins which belongs to authorized user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("users")]
        public async Task<IHttpActionResult> GetUserEntitiesLogins()
        {
            var userLogin = Request.GetLogin();
            var logins = await _queryRepository.GetUserEntitiesLoginsAsync(userLogin);
            return Ok(logins);
        }

        /// <summary>
        /// Check login for existance.
        /// </summary>
        /// <param name="login">Profile's login</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(bool))]
        [Route("check/{login}")]
        public async Task<IHttpActionResult> CheckIfLoginExist(string login)
        {
            bool isExist = await _queryRepository.LoginExistsAsync(login);
            return Ok(isExist);
        }
    }
}
