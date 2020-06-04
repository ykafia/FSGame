module Window

open System
open SDL2
open SharpBgfx

type MyWindow(name: String, width: int, height: int) =
    let mutable handle = SDL.SDL_CreateWindow(name,SDL.SDL_WINDOWPOS_CENTERED,SDL.SDL_WINDOWPOS_CENTERED,width,height,SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE)
    let mutable event = SDL.SDL_Event()
    member this.ProcessEvents(resetFlags:ResetFlags) : bool = 
        let mutable result = true
        while SDL.SDL_PollEvent(&event) <> 0 do
            match event.EventType with
            | SDL.SDL_EventType.SDL_QUIT -> 
                    printfn "Quitting window"
                    result <- false
            // | SDL.SDL_EventType.SDL_WINDOWEVENT -> 
            //     match event.window with
            //     | SDL.SDL_WindowEvent -> ()
            //     | _ -> ()
            | _ -> ()
        result
    member this.Close() = 
        SDL.SDL_DestroyWindow(handle)
        SDL.SDL_Quit()
    member this.Handle = handle


type MyGame(name : String, width : int,height : int) =
    let window = MyWindow(name,width,height)
    let height = height
    let width = width
    new() = MyGame("HelloWorld",720,480)

    member this.Height = height
    member this.Width = width
    member this.Window = window

    member this.ProcessEvents(resetFlags:ResetFlags ) =
        window.ProcessEvents(resetFlags)


