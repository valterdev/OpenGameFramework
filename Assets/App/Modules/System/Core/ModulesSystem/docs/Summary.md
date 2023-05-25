# Module System

## Quick Start

Dependencies are resolved using own IoC container.
Use `[Inject( typeof(API.System.IService) )]` or `[Inject( typeof(API.Domain.IService) )]` attribute for classes for inject dependencies in contructor.

ForExample:
```c#
public class Test()
{
    private IService1 _service1;
    private IService2 _service2;

    public Test(IService1 service1, IService2 _service2)
    {
        _service1 = service1;
        _service2 = service2;
    }

    public void SomeMethod()
    {
        _service1.ServiceMethod();
    }
}
```

For Create new Module go to Editor menu `Window -> OpenGameFramework -> Module Editor -> Create Module`

For see all modules in project go to Editor menu `Window -> OpenGameFramework -> Module Editor -> All Modules`