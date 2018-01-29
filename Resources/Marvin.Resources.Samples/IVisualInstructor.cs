﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ServiceModel;
using Marvin.AbstractionLayer.Resources;
using Marvin.Container;
using Marvin.Resources.Management;
using Marvin.Serialization;
using Marvin.Tools.Wcf;

namespace Marvin.Resources.Samples
{
    public interface IVisualInstructor : IResource
    {
        void Show(string foo);
    }

    [ResourceRegistration(nameof(VisualInstructor))]
    public class VisualInstructor : Resource, IVisualInstructor
    {
        [DataMember]
        public string ClientId { get; set; }

        [ReferenceOverride(nameof(Parent))]
        public InstructionServiceHost ServiceHost
        {
            get { return (InstructionServiceHost) Parent; }
            set { Parent = value; }
        }

        public VisualInstructor()
        {
            Descriptor = new InstructorDescriptor(this);
        }

        public void Show(string foo)
        {
            ServiceHost.DisplayFoo(ClientId, foo);
        }

        public override object Descriptor { get; }

        [EditorVisible]
        protected class InstructorDescriptor
        {
            private readonly VisualInstructor _instructor;

            public InstructorDescriptor(VisualInstructor instructor)
            {
                _instructor = instructor;
            }

            [EditorVisible]
            public string ClientId
            {
                get { return _instructor.ClientId; }
                set { _instructor.ClientId = value; }
            }

            public void Show(string foo)
            {
                _instructor.Show(foo);
            }

            [DisplayName("Clear Instructions")]
            public void Clear()
            {
                _instructor.Show(string.Empty);
            }
        }
    }

    [ResourceRegistration(nameof(AwesomeInstructor))]
    public class AwesomeInstructor : VisualInstructor
    {
        
    }

    [DependencyRegistration(typeof(IInteractionService))]
    [ResourceRegistration(nameof(InstructionServiceHost), typeof(InstructionServiceHost))]
    public class InstructionServiceHost : InteractionResource<IInteractionService>, IServiceManager
    {
        [ReferenceOverride(nameof(Children), AutoSave = true)]
        public IReferences<IVisualInstructor> Instructors { get; set; }

        [ResourceConstructor, DisplayName("Create Clients")]
        public void CreateHost([Description("Number of clients")]int clientCount,
            [ResourceTypes(typeof(VisualInstructor))]string instructorType)
        {
            DefaultConstructor();

            for (int clientNumber = 1; clientNumber <= clientCount; clientNumber++)
            {
                var instructor = Creator.Instantiate<VisualInstructor>(instructorType);
                instructor.Name = instructor.ClientId = $"Client{clientNumber}";
                Instructors.Add(instructor);
            }
        }

        [ResourceConstructor(IsDefault = true)]
        public void DefaultConstructor()
        {
            HostConfig = new HostConfig
            {
                Endpoint = "AssemblyInstructions",
                BindingType = ServiceBindingType.NetTcp,
                MetadataEnabled = true,
            };
        }

        public void Register(ISessionService service)
        {
            throw new System.NotImplementedException();
        }

        public void Unregister(ISessionService service)
        {
            throw new System.NotImplementedException();
        }

        public void DisplayFoo(string client, string message)
        {
        }
    }


    [ServiceContract]
    [ServiceVersion(ServerVersion = "1.0.0", MinClientVersion = "1.0.0")]
    public interface IInteractionService : ISessionService
    {
    }

    [Plugin(LifeCycle.Transient, typeof(IInteractionService))]
    public class InteractionService : SessionService<InstructionServiceHost>, IInteractionService
    {

    }
}