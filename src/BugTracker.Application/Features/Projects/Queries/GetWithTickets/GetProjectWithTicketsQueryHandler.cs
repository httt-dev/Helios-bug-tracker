﻿using AutoMapper;
using BugTracker.Application.Contracts.Data;
using BugTracker.Application.Responses;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BugTracker.Application.Features.Projects.Queries.GetWithTickets
{
    public class GetProjectWithTicketsQueryHandler : IRequestHandler<GetProjectWithTicketsQuery, ApiResponse<ProjectWithTicketsVm>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public GetProjectWithTicketsQueryHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<ApiResponse<ProjectWithTicketsVm>> Handle(GetProjectWithTicketsQuery request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<ProjectWithTicketsVm>();
            var project = await _projectRepository.GetByIdAsync(request.Id);
            if (project == null)
            {
                return response.setNotFoundResponse($"Category with id {request.Id} was not found");
            }
            response.Data = _mapper.Map<ProjectWithTicketsVm>(project);
            return response;
        }
    }
}
