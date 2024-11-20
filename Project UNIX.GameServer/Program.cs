using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ProjectUNIX.GameServer.Network.Hosting;
using ProjectUNIX.GameServer.Network.Manager;
using Project_UNIX.Common.Validator.Token;
using Project_UNIX.Common.Database;
using Project_UNIX.Common.Server;
using ProjectUNIX.GameServer.Game.Event;
using ProjectUNIX.GameServer.Game.Player;
using ProjectUNIX.GameServer.Utils.DataCollection;
using ProjectUNIX.GameServer.Utils.DataCollection.Assets;
using ProjectUNIX.GameServer.Network.NetCommand;
using ProjectUNIX.GameServer.Network.kcp.Session;
using ProjectUNIX.GameServer.Utils.DataCollection.Quests;
using ProjectUNIX.GameServer.Network.kcp;
using ProjectUNIX.GameServer.Game.GameManager;
using ProjectUNIX.GameServer.Game.World;

// Host Builder
Console.Title = "GameServer";

HostApplicationBuilder builder = Host.CreateApplicationBuilder();
builder.Logging.AddSimpleConsole();

//Data 
builder.Services.AddSingleton<IAssets,LocalAssets>();
builder.Services.AddSingleton<ExcelDataCollectionTable>();
builder.Services.AddSingleton<QuestDataCollectionTable>();
builder.Services.AddSingleton<BinaryDataCollectionTable>();

// Game Logic
builder.Services.AddScoped<GamePlayer>();
builder.Services.AddScoped<PlayerWorld>();
builder.Services.AddScoped<EntityModuleManager>();
builder.Services.AddScoped<SceneModuleManager>();
builder.Services.AddSingleton<PlayerWorldManager>();

//Logic Event
builder.Services.AddScoped<IEntityEventListener, SessionEntityListener>();

// Network
builder.Services.AddScoped<NetCommandDispatcher>();
builder.Services.AddScoped<NetSession,KcpSession>();
builder.Services.AddSingleton<IGateway, KcpGateway>();
builder.Services.AddSingleton<NetSessionManager>();

//Tool
builder.Services.AddSingleton<TokenValidation>();

//Database
builder.Services.AddSingleton<DatabaseHandler>();
builder.Services.AddSingleton<Account>();

builder.Services.AddHostedService<GameServer>();

await builder.Build().RunAsync();