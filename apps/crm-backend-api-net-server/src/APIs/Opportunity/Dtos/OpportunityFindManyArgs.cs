using CrmBackendApiNet.APIs.Common;
using CrmBackendApiNet.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrmBackendApiNet.APIs.Dtos;

[BindProperties(SupportsGet = true)]
public class OpportunityFindManyArgs : FindManyInput<Opportunity, OpportunityWhereInput> { }
