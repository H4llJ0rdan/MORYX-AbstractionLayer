---
uid: Capabilities
---
# Capabilities

[Capabilities](xref:Moryx.AbstractionLayer.Capabilities.ICapabilities) are basically a self description of a [Resource](../Resources/Overview.md). Every [Activity](Activities.md) defines which capabilities it needs and the provided information will be used to find a matching Resource to handle the activity. It is also possible that a resource has multiple capabilities and is able to handle various activities.

## Single Capabilities

Just implement the interface to define a capability like in the example:

```` cs
[DataContract]
public class MyCapabilities : ICapabilities
{
    public bool IsCombined => false;

    public bool ProvidedBy(ICapabilities provided) => provided is MyCapabilities;

    public bool Provides(ICapabilities required) => required is MyCapabilities;

    public IEnumerable<ICapabilities> GetAll()
    {
        yield return this;
    }
}
````

It is also possible to use the base class [ConcreteCapabilities](xref:Moryx.AbstractionLayer.Capabilities.ConcreteCapabilities), which reduces the code to the following lines:

```` cs
[DataContract]
public class MyCapabilities : ConcreteCapabilities
{
    protected override bool ProvidedBy(ICapabilities provided) => provided is MyCapabilities;
}
````

In any case you can extend your capabilities with more properties to give the resource are more meaningfull self description. Let's take the ScrewingCapabilities as an example. Maybe there is more than one station which has ScrewingCapabilities but each can handle a different screw head. Our Capability implementation could then look like:

```` cs
public enum ScrewHead
{
    Default, 
    Phillips
}

[DataContract]
public class ScrewingCapabilities : AssembleCapabilities
{
    [DataMember]
    public ScrewHead Head { get; set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public ScrewingCapabilities()
    {    
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ScrewingCapabilities"/> class.
    /// </summary>
    public ScrewingCapabilities(ScrewHead providedHead)
    {
        Head = providedHead;
    }

    protected override bool ProvidedBy(ICapabilities provided)
    {
        var capabilities = provided as ScrewingCapabilities;
        return capabilities != null && capabilities.Head == Head;
    }
}
````

So it is possible to extend a Capability with different information to distinguish between resources with the same capabilities.

## Multiple Capabilities

A resource can also have multiple capabilities. For that the [CombinedCapabilities](xref:Moryx.AbstractionLayer.Capabilities.CombinedCapabilities) class should be used, which implementes the ICapabilities interface as well. Thus, it is possible to set the resource capabilities to a list of capabilities like in the following example:

```` cs
// some resource code

public override void Initialize()
{
    base.Initialize();

    Capabilities = new CombinedCapabilities(new List<ICapabilities>
    {
        new MyCapabilities(),
        new ScrewingCapabilities(ScrewHead.Phillips),
        new CoolCapabilities(),
        new HotCapabilities(),
        new LitCapabilities()
    });
}

// some more resource code
````
