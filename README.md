## Deploy

create windows service
```powershell
sc.exe create WorkerServiceDemo binpath= c:\path\WorkerSericeDemo.exe start=auto
```

delete windows service
```powershell
sc.exe delete WorkerServiceDemo
```