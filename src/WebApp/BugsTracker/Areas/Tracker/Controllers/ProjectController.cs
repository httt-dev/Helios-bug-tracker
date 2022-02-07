﻿using BugTracker.Application.Dto.Projects;
using BugTracker.Application.Features.Projects.Commands.Create;
using BugTracker.Application.Features.Projects.Commands.Update;
using BugTracker.Application.Features.Projects.Queries.Get;
using BugTracker.Application.Features.Projects.Queries.GetWithTeam;
using BugTracker.Application.Features.Team.Queries.GetAllAccessibleMembers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BugTracker.Areas.Tracker.Controllers
{
    [Area("Tracker")]
    public class ProjectController : BaseController
    {
        private const string ModalBasePath = "~/Areas/Tracker/Views/Shared/Partial/Project/";
        private const string ModalType = "ProjectModalPartial.cshtml";
        private const string CreateModalPath = ModalBasePath + "_create" + ModalType;
        private const string UpdateModalPath = ModalBasePath + "_update" + ModalType;
        private const string DeleteModalPath = ModalBasePath + "_delete" + ModalType;

        [HttpPost]
        public async Task<IActionResult> Create([Bind(Prefix = "Command")] CreateProjectCommand command)
        {
            var response = await Mediator.Send(command);
            if (!response.Succeeded)
            {
                return RedirectToAction("Dashboard", "Home", new { isFailed = true });

            }
            return RedirectToAction("Dashboard", "Home", new { isSuccess = true });
        }

        public async Task<IActionResult> Update([Bind(Prefix = "Command")] UpdateProjectCommand command)
        {
            var response = await Mediator.Send(command);
            if (!response.Succeeded)
            {
                return RedirectToAction("Dashboard", "Home", new { isFailed = true });

            }
            return RedirectToAction("Dashboard", "Home", new { isSuccess = true });
        }

        public async Task<IActionResult> LoadCreateModal()
        {
            var dto = new CreateProjectDto();
            var response = await Mediator.Send(new GetAllAccessibleMembersQuery());
            dto.Team = response.DataList;

            return PartialView(CreateModalPath, dto);
        }

        public async Task<IActionResult> LoadUpdateModal(Guid id)
        {
            var dto = new UpdateProjectDto();
            dto.Command = new UpdateProjectCommand();
            var teamResponse = await Mediator.Send(new GetAllAccessibleMembersQuery());
            var projectResponse = await Mediator.Send(new GetProjectWithTeamQuery() {Id = id });
            dto.Team = teamResponse.DataList;
            dto.Command.Id = projectResponse.Data.Id;
            dto.Command.Name = projectResponse.Data.Name;
            dto.Command.Description = projectResponse.Data.Description;
            dto.Command.Team = projectResponse.Data.TeamIds;

            return PartialView(UpdateModalPath, dto);
        }

        public async Task<IActionResult> LoadDeleteModal(Guid id)
        {
            var response = await Mediator.Send(new GetProjectQuery(id));
            return PartialView(DeleteModalPath, response.Data);
        }
    }
}
