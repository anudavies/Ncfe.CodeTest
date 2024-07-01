﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncfe.CodeTest.Interfaces
{
    public interface ILearnerDataAccess
    {
        Task<LearnerResponse> LoadLearnerAsync(int learnerId);
    }
}
