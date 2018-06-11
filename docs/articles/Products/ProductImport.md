# Product Import

Product import includes any type of product type definition except model setups. Product import may refer to importing a file or remote source, but also the simple act of creating a product manually by supplying all necessary parameters.

Product imports of a machine are created by implementing the [IProductImporter](@ref Marvin.Products.IProductImporter). As always it is recommended to derive from the base type [ProductImportBase](@ref Marvin.Products.Management.ProductImporterBase). Each importer must define its [ProductImporterConfig](@ref Marvin.Products.ProductImporterConfig) and export a typed parameters object that implements `IImportParameters`. This object is exchanged between client and server using the Entry-format. There are already two predefined types [FileImportParameters](@ref Marvin.Products.Management.FileImportParameters) and [PrototypeImportParameters](@ref Marvin.Products.Management.PrototypeImportParameters) that can be used directly or as a base class for your parameters

The base class requires three methods:

* **GenerateParameters:** This override is optional in case your parameters object requires more initialization than a simple constructor invocation. Otherwise you can leave the default behavior.
* **UpdateParameters:** Some parameters where entered by the user and the importer can now update, add or modify other parameters. This provides an interactive workflow between the client and importer plugin. For example an importer could prefill the revision field after a material number was entered.
* **Import:** Parse the parameters from the client and construct an `IProduct` instance. **DO NOT** write this to the database, but return the unsaved object instead.

It is important that the new created importer is configured in the **Maintenance -> ProductManager -> Importers**.
Otherwise it will not be shown on the client. Unless your application defines its implementation of `IProductCustomatization` use must enable `UseNullCustomization` in the Maintenance as well. 

## Prototype importer

A simple prototype importer for our watch product model is shown below. Like other plugins it needs to define a config and export it as `ExpectedConfig` above the class. The `ImporterType.Prototype` defines that this importer creates an empty product instance that can later be finalized using the default product editor.

````cs
[DataContract]
public class PrototypeImporterConfig : ProductImporterConfig
{
    public override string PluginName => nameof(PrototypeImporter);
}

public class WatchImportParameters : PrototypeParameters
{
    [Required, TriggersUpdate]
    [PossibleProductValues(nameof(WatchProduct), nameof(WatchfaceProduct), nameof(NeedleProduct))]
    public string ProductType { get; set; }
}

[ExpectedConfig(typeof(PrototypeImporterConfig))]
[Plugin(LifeCycle.Singleton, typeof(IProductImporter), Name = nameof(PrototypeImporter))]
public class PrototypeImporter : ProductImporterBase<PrototypeImporterConfig, WatchImportParameters>
{
    protected override WatchImportParameters Update(WatchImportParameters currentParameters)
    {
        if (string.IsNullOrEmpty(currentParameters.Name) && !string.IsNullOrEmpty(currentParameters.ProductType))
        {
            currentParameters.Name = currentParameters.ProductType + "_";
        }

        return currentParameters;
    }

    protected override IProduct[] Import(WatchImportParameters parameters)
    {
        Product product = null;
        switch (parameters.ProductType)
        {
            case nameof(WatchProduct):
                product = new WatchProduct
                {
                    Needles = new List<NeedlePartLink>()
                };
                break;
            case nameof(WatchfaceProduct):
                product = new WatchfaceProduct
                {
                    Numbers = new[] { 3, 6, 9, 12 }
                };
                break;
            case nameof(NeedleProduct):
                product = new NeedleProduct();
                break;
        }

        var identifier = parameters.Identifier;
        var rev = parameters.Revision;
        product.Identity = new ProductIdentity(identifier, rev);
        product.Name = parameters.Name;

        return new IProduct[] { product };
    }
}
````

## File Importer

In many applications the products are already defined in some digital form. This may be XML, Excel or a proprietary format with a parser. In these cases it makes sense to implement a file-based product importer to make the transition to the new application easier. File importers are very similar to the previous example of a prototype importer. The main difference are the generated parameters and how they are converted into a product in the `Import` method.

The client will send a file path or encode the file as a Base64 string into the `FileProperty`. Using the `ReadFile`-method on the parameters will either open the file or parse the Base64 string. This makes the importer independent from the clients location and behavior.

````cs
[DataContract]
public class FileImporterConfig : ProductImporterConfig
{
    public override string PluginName => nameof(FileImporter);
}

[ExpectedConfig(typeof(FileImporterConfig))]
[Plugin(LifeCycle.Singleton, typeof(IProductImporter), Name = nameof(FileImporter))]
public class FileImporter : ProductImporterBase<FileImporterConfig>
{
    protected override IImportParameters GenerateParameters()
    {
        return new FileImportParameters { FileExtension = ".mjb" };
    }

    protected override IProduct[] Import(FileImportParameters parameters)
    {
        using (var stream = parameters.ReadFile())
        {
            var textReader = new StreamReader(stream);
            var identifier = textReader.ReadLine();
            var revision = short.Parse(textReader.ReadLine() ?? "0");
            var name = textReader.ReadLine();

            return new IProduct[]
            {
                new NeedleProduct
                {
                    Name = name,
                    Identity = new ProductIdentity(identifier, revision)
                }
            };
        }
    }
}
````