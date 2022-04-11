﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class ValidationConditionRepository : BasicRepository<ValidationCondition>, IValidationConditionRepository
    {
        public ValidationConditionRepository(X1APEntities dbContext) : base(dbContext)
        {
        }
    }
}
