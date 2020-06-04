module GPGPU

open System
open ILGPU
open ILGPU.Runtime

let invoke (action : Action<'a,'b,'c>) (args : 'a*'b*'c) = 
    action.Invoke(args)

let MyKernel (index : Index1)(dataView : ArrayView<int>)(constant : int) =
    dataView.[index] <- constant + int index


let sample = 
    use mutable context = new Context()
    for accelID in Accelerator.Accelerators do
        use mutable accel = Accelerator.Create(context,accelID)
        printfn "Performing operations on %A" accel
        let mutable kernel = accel.LoadAutoGroupedStreamKernel<Index1, ArrayView<int>, int>(MyKernel)
        use mutable buffer = accel.Allocate<int>(1024)
        invoke kernel (Index1(buffer.Length),buffer.View,42)
        accel.Synchronize()
        buffer.GetAsArray()
            |> Array.iteri (fun i x -> if x <> 42 then printfn "Error at element location %d : %A found" i x)
        printf "Done"
