# dtmcli-csharp-sample
dtmcli c# 使用示例

## 快速开始
### 部署启动dtm
需要docker版本18以上
```
git clone https://github.com/yedf/dtm
cd dtm
docker-compose up
```
### 启动示例
```
cd DtmTccSample
dotnet run DtmTccSample.csproj
```
### 运行示例

使用浏览器访问 http://192.168.5.9:5000/swagger/index.html

执行 ​/api​/Home​/Demo

### 示例解读
核心代码如下，示例开启一个tcc全局事务，然后在事务内部注册并调用TransOut和TransIn 分支，完成后返回，剩下的二阶段Comfirm，会由DTM完成
```C# 

var svc = "http://192.168.5.9:5000/api";
TransRequest request = new TransRequest() { Amount = 30 };
var cts = new CancellationTokenSource();
// 开启一个tcc全局事务   
await globalTransaction.Excecute(async (tcc) =>
{
     //调用TransOut分支，四个参数分别为post的body，tryUrl, confirmUrl, cancelUrl
      await tcc.CallBranch(request, svc + "/TransOut/Try", svc + "/TransOut/Confirm", svc + "/TransOut/Cancel", cts.Token);
      //调用TransIn分支
      await tcc.CallBranch(request, svc + "/TransIn/Try", svc + "/TransIn/Confirm", svc + "/TransIn/Cancel",cts.Token);
    }, cts.Token);
}
```