﻿// Copyright 2007-2016 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.NinjectIntegration
{
    using System.Threading.Tasks;
    using GreenPipes;
    using Ninject;
    using Pipeline;
    using Saga;


    public class NinjectSagaRepository<TSaga> :
        ISagaRepository<TSaga>
        where TSaga : class, ISaga
    {
        readonly IKernel _kernel;
        readonly ISagaRepository<TSaga> _repository;

        public NinjectSagaRepository(ISagaRepository<TSaga> repository, IKernel kernel)
        {
            _repository = repository;
            _kernel = kernel;
        }

        void IProbeSite.Probe(ProbeContext context)
        {
            var scope = context.CreateScope("ninject");

            _repository.Probe(scope);
        }

        public Task Send<T>(ConsumeContext<T> context, ISagaPolicy<TSaga, T> policy, IPipe<SagaConsumeContext<TSaga, T>> next) where T : class
        {
            return _repository.Send(context, policy, next);
        }

        public Task SendQuery<T>(SagaQueryConsumeContext<TSaga, T> context, ISagaPolicy<TSaga, T> policy, IPipe<SagaConsumeContext<TSaga, T>> next)
            where T : class
        {
            return _repository.SendQuery(context, policy, next);
        }
    }
}