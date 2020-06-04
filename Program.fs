open System
open SharpBgfx
open SDL2
open Window

let initBgfx (game:MyGame)= 
    Bgfx.Init() |> ignore
    Bgfx.Reset(game.Width, game.Height, ResetFlags.Vsync);

    // enable debug text
    Bgfx.SetDebugFeatures(DebugFeatures.DisplayText)

    // set view 0 clear state
    Bgfx.SetViewClear(uint16 0, ClearTargets.Color ||| ClearTargets.Depth,uint32 0x303030ff);


let renderFunction (game : MyGame) = 
    async {
        // main loop
        printfn "Starting render loop"
        while game.ProcessEvents(ResetFlags.Vsync) do
            // set view 0 viewport
            Bgfx.SetViewRect(uint16 0, 0, 0, game.Width, game.Height) 

            //  make sure view 0 is cleared if no other draw calls are submitted
            Bgfx.Touch(uint16 0) |> ignore 

            // write some debug text
            Bgfx.DebugTextClear();

            Bgfx.DebugTextWrite(0, 1, DebugColor.White, DebugColor.Blue, "It's my game");
            Bgfx.DebugTextWrite(0, 2, DebugColor.White, DebugColor.Cyan, "Description: Initialization and debug text.");

            // advance to the next frame. Rendering thread will be kicked to
            // process submitted rendering primitives.
            Bgfx.Frame() |> ignore
        

        // clean up
        Bgfx.Shutdown();
    }
    


[<EntryPoint>]
let main argv =
    SDL.SDL_Init(SDL.SDL_INIT_VIDEO) |> ignore
    let game = MyGame()
    initBgfx game
    Bgfx.SetWindowHandle(game.Window.Handle)    
    renderFunction game |> Async.RunSynchronously
    let mutable quit = false

    while not quit do
        Bgfx.ManuallyRenderFrame() |> ignore
        quit <- game.ProcessEvents(ResetFlags.Vsync)
        

    0


