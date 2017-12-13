﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common;
using Funkmap.Common.Auth;
using Funkmap.Common.Models;
using Funkmap.Data.Entities;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;
using Funkmap.Models;
using Funkmap.Models.Requests;
using Funkmap.Services.Abstract;

namespace Funkmap.Controllers
{
    [RoutePrefix("api/band")]
    public class BandController: ApiController
    {
        private readonly IBandRepository _bandRepository;
        private readonly IBaseRepository _baseRepository;
        private readonly IDependenciesController _dependenciesController;


        public BandController(IBandRepository bandRepository, IBaseRepository baseRepository, IDependenciesController dependenciesController)
        {
            _bandRepository = bandRepository;
            _baseRepository = baseRepository;
            _dependenciesController = dependenciesController;
        }
        
        [HttpPost]
        [Authorize]
        [Route("getInviteInfo")]
        public async Task<IHttpActionResult> GetInviteMusicianInfo(BandInviteInfoRequest request)
        {
            var login = Request.GetLogin();

            var parameter = new CommonFilterParameter()
            {
                EntityType = EntityType.Band,
                UserLogin = login,
                Skip = 0,
                Take = Int32.MaxValue
            };
            var bandEntities = await _baseRepository.GetFilteredAsync(parameter);
            var availableBands = bandEntities.Cast<BandEntity>()
                .Where(x=>(x.MusicianLogins == null && x.InvitedMusicians == null) 
                    || ((x.MusicianLogins == null || !x.MusicianLogins.Contains(request.InvitedMusician))) 
                    && (x.InvitedMusicians == null || !x.InvitedMusicians.Contains(request.InvitedMusician)))
                .Select(x=>x.ToPreviewModel()).ToList();

            var info = new BandInviteInfo()
            {
                AvailableBands = availableBands
            };
            return Ok(info);
        }

        [Authorize]
        [HttpPost]
        [Route("removeMusician")]
        public async Task<IHttpActionResult> RemoveMusicianFromBand(UpdateBandMembersRequest membersRequest)
        {
            var userLogin = Request.GetLogin();
            var band = await _bandRepository.GetAsync(membersRequest.BandLogin);
            if (band.UserLogin != userLogin) return BadRequest("is not your band");

            var parameter = new CleanDependenciesParameter()
            {
                EntityType = EntityType.Musician,
                EntityLogin = membersRequest.MusicianLogin,
                FromEntityLogin = membersRequest.BandLogin
            };
            await _dependenciesController.CleanDependenciesAsync(parameter);

            return Ok(new BaseResponse() { Success = true });
        }
    }
}
