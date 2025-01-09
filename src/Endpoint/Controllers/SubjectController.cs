using
 Application.Interfaces;
using Application.UseCases.Subjects;
using AutoMapper;
using Domain.Enums;
using Endpoint.ApiRequests.Subjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly IMapper _mapper;

        public SubjectController(ISubjectService subjectService, IMapper mapper)
        {
            _subjectService = subjectService;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GetSubjectsRequest, GetSubjectsParams>();
                cfg.CreateMap<CreateSubjectRequest, CreateSubjectParams>();
                cfg.CreateMap<UpdateSubjectRequest, UpdateSubjectParams>();
                cfg.CreateMap<CreateDeleteSubjectKnowledgeRequest, CreateDeleteSubjectKnowledgeParams>();
            });
            _mapper = config.CreateMapper();
        }

        [HttpPost(HttpRoute.GetSubjects)]
        [Authorize]
        public async Task<IActionResult> GetSubjects([FromBody] GetSubjectsRequest request)
        {
            var Params = _mapper.Map<GetSubjectsParams>(request);

            var result = await _subjectService.GetSubjects(Params);

            return result.IsSuccess
                ? Ok(new
                {
                    data = result.Value,
                    paging = result.Paging
                })
                : BadRequest(result.Errors);
        }

        [HttpGet(HttpRoute.GetSubjectById)]
        [Authorize]
        public async Task<IActionResult> GetSubjectById(Guid id)
        {
            var result = await _subjectService.GetSubjectByGuid(id);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.CreateSubject)]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> CreateSubject([FromForm] CreateSubjectRequest request)
        {
            var Params = _mapper.Map<CreateSubjectParams>(request);

            var result = await _subjectService.CreateSubject(Params);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.UpdateSubject)]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> UpdateSubject([FromForm] UpdateSubjectRequest request)
        {
            var Params = _mapper.Map<UpdateSubjectParams>(request);

            var result = await _subjectService.UpdateSubject(Params);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Errors);
        }

        [HttpDelete(HttpRoute.DeleteSubject)]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> DeleteSubject(Guid id)
        {
            var result = await _subjectService.DeleteSubject(id);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.CreateDeleteSubjectKnowledge)]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> CreateDeleteSubjectKnowledge([FromBody] CreateDeleteSubjectKnowledgeRequest request)
        {
            var Params = _mapper.Map<CreateDeleteSubjectKnowledgeParams>(request);

            var result = await _subjectService.CreateDeleteSubjectKnowledge(Params);

            return result.IsSuccess
                ? Ok(result.Value.ToString())
                : BadRequest(result.Errors);
        }
    }
}
