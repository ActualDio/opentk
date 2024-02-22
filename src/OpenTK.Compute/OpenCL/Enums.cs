using System;
using System.Runtime.InteropServices;
using static System.Net.WebRequestMethods;

namespace OpenTK.Compute.OpenCL
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    #region Context
    public enum ContextProperties : int
    {
        ContextPlatform = 0x1084,
        InteropUserSync = 0x1085,

        GlContextKHR = 0x2008,
        EglDisplayKHR = 0x2009,
        GlxDisplayKHR = 0x200A,
        WglHDCKHR = 0x200B,
        CglShareGroupKHR = 0x200C,

        D3D10DeviceKHR = 0x4014,

        AdapterD3D9KHR = 0x2025,
        AdapterD3D9EXKHR = 0x2026,
        AdapterDXVAKHR = 0x2027,

        D3D11DeviceKHR = 0x401D,

        MemoryInitializeKHR = 0x2030,
        TerminateKHR = 0x2032,
    }

    public enum ContextInfo : uint
    {
        ReferenceCount = 0x1080,
        Devices = 0x1081,
        Properties = 0x1082,
        NumberOfDevices = 0x1083
    }

    #endregion

    #region Platform

    /// <summary>
    /// The information that can be queried using <c><see cref="CL.GetPlatformInfo(CLPlatform, PlatformInfo, out byte[])">GetPlatformInfo()</see></c>.
    /// <para>Original documentation: https://registry.khronos.org/OpenCL/specs/3.0-unified/html/OpenCL_API.html#platform-queries-table.</para>
    /// </summary>
    public enum PlatformInfo : uint
    {
        /// <summary>
        /// OpenCL profile string. Returns the profile name supported by the implementation.
        /// The profile name returned can be one of the following strings:
        ///
        /// <list type="bullet">
        /// <item><term>
        /// FULL_PROFILE</term>
        /// <description>
        /// if the implementation supports the OpenCL specification
        /// (functionality defined as part of the core specification and does not require any extensions to be supported).
        /// </description></item>
        ///
        /// <item><term>
        /// EMBEDDED_PROFILE</term>
        /// <description>
        /// if the implementation supports the OpenCL embedded profile.
        /// The embedded profile for OpenCL is described in:
        /// https://www.khronos.org/registry/OpenCL/specs/3.0-unified/html/OpenCL_API.html#opencl-embedded-profile.
        /// </description></item>
        /// </list>
        /// </summary>
        /// <remarks>Return Type: string</remarks>
        Profile = 0x0900,

        /// <summary>
        /// OpenCL version string. Returns the OpenCL version supported by the implementation.
        /// This version string has the following format:
        /// <para/>
        /// <para>"OpenCL {major_version.minor_version} {platform-specific information}"</para>
        /// The major_version.minor_version value returned will be one of 1.0, 1.1, 1.2, 2.0, 2.1, 2.2 or 3.0.
        /// </summary>
        /// <remarks>Return Type: string</remarks>
        Version = 0x0901,

        /// <summary>
        /// Platform name string.
        /// </summary>
        /// <remarks>Return Type: string</remarks>
        Name = 0x0902,

        /// <summary>
        /// Platform vendor string.
        /// </summary>
        /// <remarks>Return Type: string</remarks>
        Vendor = 0x0903,

        /// <summary>
        /// Returns a space separated list of extension names (the extension names themselves do not contain any spaces)~
        /// supported by the platform. Each extension that is supported by all devices associated with this
        /// platform must be reported here.
        /// </summary>
        /// <remarks>Return Type: string</remarks>
        Extensions = 0x0904,

        /// <summary>
        /// Introduced in OpenCL 2.1.
        /// Returns the resolution of the host timer in nanoseconds as used by
        /// <c><see cref="CL.GetHostTimer(CLDevice, IntPtr)">GetHostTimer()</see></c>.
        /// This value must be 0 for devices that do not support device and host timer synchronization.
        /// </summary>
        /// <remarks>Return Type: ulong</remarks>
        PlatformHostTimerResolution = 0x0905,

        /// <summary>
        /// Only available for OpenCL 2.0 and 2.1
        /// If the cl_khr_icd extension is enabled, the function name suffix used to identify extension
        /// functions to be directed to this platform by the ICD Loader.
        /// <para/>
        /// <para>For more see: https://registry.khronos.org/OpenCL/sdk/2.1/docs/man/xhtml/clGetPlatformInfo.html</para>
        /// </summary>
        /// <remarks>Return Type: string</remarks>
        PlatformIcdSuffix = 0x0920
    }

    #endregion

    #region Device

    /// <summary>
    /// The valid types of OpenCL devices.
    /// <para>
    /// The device type is purely informational and has no semantic meaning.
    /// </para>
    /// <para>
    /// Some devices may be more than one type. For example,
    /// a <c><see cref="Cpu">CPU</see></c> device may also be a <c><see cref="Gpu">GPU</see></c> device,
    /// or a <c><see cref="Accelerator">Accelerator</see></c> device may also be some other, more descriptive device type.
    /// <c><see cref="Custom">Custom</see></c> devices must not be combined with any other device types.
    /// </para>
    /// <para>
    /// One device in the platform should be a <c><see cref="Default">Default</see></c> device.
    /// The default device should also be a more specific device type,
    /// such as <c><see cref="Cpu">CPU</see></c> or <c><see cref="Gpu">GPU</see></c>.
    /// </para>
    /// </summary>
    [Flags]
    public enum DeviceType : ulong
    {
        /// <summary>
        /// The default OpenCL device in the platform. The default OpenCL device must not be a
        /// <c><see cref="Custom">Custom</see></c> device.
        /// </summary>
        Default = 1 << 0,

        /// <summary>
        /// An OpenCL device similar to a traditional CPU (Central Processing Unit).
        /// The host processor that executes OpenCL host code may also be considered a CPU OpenCL device.
        /// </summary>
        Cpu = 1 << 1,

        /// <summary>
        /// An OpenCL device similar to a GPU (Graphics Processing Unit).
        /// Many systems include a dedicated processor for graphics or
        /// rendering that may be considered a GPU OpenCL device.
        /// </summary>
        Gpu = 1 << 2,

        /// <summary>
        /// Dedicated devices that may accelerate OpenCL programs,
        /// such as FPGAs (Field Programmable Gate Arrays),
        /// DSPs (Digital Signal Processors), or AI (Artificial Intelligence) processors.
        /// </summary>
        Accelerator = 1 << 3,

        /// <summary>
        /// Specialized devices that implement some of the OpenCL runtime
        /// APIs but do not support all required OpenCL functionality.
        /// </summary>
        /// <remarks>Only available after OpenCL 1.2</remarks>
        Custom = 1 << 4,

        /// <summary>
        /// All OpenCL devices available in the platform,
        /// except for <c><see cref="Custom">Custom</see></c> devices.
        /// </summary>
        All = 0xFFFFFFFF
    }

    /// <summary>
    /// <para>
    ///     The information that can be queried using
    ///     <c><see cref="CL.GetDeviceInfo(CLDevice, DeviceInfo, out byte[])">GetDeviceInfo()</see></c>.
    /// </para>
    /// <para>
    ///     For more info look at the original documentation
    ///     <b><u><see href="https://registry.khronos.org/OpenCL/specs/3.0-unified/html/OpenCL_API.html#device-queries-table">here</see></u></b>.
    /// </para>
    /// </summary>
    public enum DeviceInfo : ulong
    {
        /// <summary>
        /// <para>
        ///     The type or types of the OpenCL device. Please see <c><see cref="DeviceType">DeviceType</see></c>
        ///     for supported device types and device type combinations.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="DeviceType">DeviceType</see></c>
        /// </para>
        /// </summary>
        Type = 0x1000,

        /// <summary>
        /// <para>
        ///     A unique device vendor identifier.
        /// </para>
        /// <para>
        ///     If the vendor has a PCI vendor ID, the low 16 bits must contain that PCI vendor ID,
        ///     and the remaining bits must be set to zero. Otherwise, the value returned must be a
        ///     valid Khronos vendor ID represented by type <c>cl_khronos_vendor_id</c>.
        ///     Khronos vendor IDs are allocated starting at 0x10000, to distinguish them from the PCI vendor ID namespace.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        VendorId = 0x1001,

        /// <summary>
        /// <para>
        ///     The number of parallel compute units on the OpenCL device.
        ///     A work-group executes on a single compute unit. The minimum value is 1.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        MaximumComputeUnits = 0x1002,

        /// <summary>
        /// <para>
        ///     Maximum dimensions that specify the global and local work-item IDs used by the data parallel execution model.
        ///     (Refer to
        ///     <c><see cref="CL.EnqueueNDRangeKernel(CLCommandQueue,
        ///     CLKernel, uint, UIntPtr[], UIntPtr[], UIntPtr[], uint,
        ///     CLEvent[], out CLEvent)">EnqueueNDRangeKernel()</see></c>).
        ///     The minimum value is 3 for devices that are not of type
        ///     <c><see cref="DeviceType.Custom">DeviceType.Custom</see></c>.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>UIntPtr</c>
        /// </para>
        /// </summary>
        MaximumWorkItemDimensions = 0x1003,

        /// <summary>
        /// <para>
        ///     Maximum number of work-items that can be specified in each dimension of the work-group to
        ///     <c><see cref="CL.EnqueueNDRangeKernel(CLCommandQueue,
        ///     CLKernel, uint, UIntPtr[], UIntPtr[], UIntPtr[], uint,
        ///     CLEvent[], out CLEvent)">EnqueueNDRangeKernel()</see></c>.
        ///     Returns n <c>UIntPtr</c> entries, where n is the value returned by the query for
        ///     <c><see cref="MaximumWorkItemDimensions">MaximumWorkItemDimensions</see></c>.
        ///     The minimum value is (1, 1, 1) for devices that are not of type
        ///     <c><see cref="DeviceType.Custom">Custom</see></c>.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>UIntPtr</c>
        /// </para>
        /// </summary>
        MaximumWorkItemSizes = 0x1005,

        /// <summary>
        /// <para>
        ///     Maximum number of work-items in a work-group that a device is capable
        ///     of executing on a single compute unit, for any given kernel-instance running on the device.
        ///     (Refer to <c><see cref="CL.EnqueueNDRangeKernel(CLCommandQueue,
        ///     CLKernel, uint, UIntPtr[], UIntPtr[], UIntPtr[], uint,
        ///     CLEvent[], out CLEvent)">EnqueueNDRangeKernel()</see></c> and
        ///     <c><see cref="KernelWorkGroupInfo.WorkGroupSize">WorkGroupSize</see></c>).
        ///     The minimum value is 1.
        /// </para>
        /// <para>
        ///     The returned value is an upper limit and will not necessarily maximize performance.
        ///     This maximum may be larger than supported by a specific kernel
        ///     (refer to the <c><see cref="KernelWorkGroupInfo.WorkGroupSize">WorkGroupSize</see></c>
        ///     query of <c><see cref="CL.GetKernelWorkGroupInfo(CLKernel, CLDevice,
        ///     KernelWorkGroupInfo, UIntPtr, byte[], out UIntPtr)">GetKernelWorkGroupInfo()</see></c>).
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>UIntPtr</c>
        /// </para>
        /// </summary>
        MaximumWorkGroupSize = 0x1004,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before version 1.1.</pre></i>
        /// </para>
        /// <para>
        ///     Preferred native vector width size for built-in scalar types that can be put into vectors.
        /// </para>
        /// <para>
        ///     The vector width is defined as the number of scalar elements that can be stored in the vector.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        PreferredVectorWidthChar = 0x1006,

        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        PreferredVectorWidthShort = 0x1007,

        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        PreferredVectorWidthInt = 0x1008,

        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        PreferredVectorWidthLong = 0x1009,

        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        PreferredVectorWidthFloat = 0x100A,

        /// <summary>
        /// <para>
        ///     Preferred native vector width size for built-in scalar types that can be put into vectors.
        /// </para>
        /// <para>
        ///     The vector width is defined as the number of scalar elements that can be stored in the vector.
        /// </para>
        /// <para>
        ///     If double precision is not supported, must return 0.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        PreferredVectorWidthDouble = 0x100B,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing byefore verison 1.1.</pre></i>
        /// </para>
        /// <para>
        ///     Preferred native vector width size for built-in scalar types that can be put into vectors.
        /// </para>
        /// <para>
        ///     The vector width is defined as the number of scalar elements that can be stored in the vector.
        /// </para>
        /// <para>
        ///     If the <c>cl_khr_fp16</c> extension is not supported, must return 0.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        PreferredVectorWidthHalf = 0x1034,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.1.</pre></i>
        /// </para>
        /// <para>
        ///     Returns the native ISA vector width.
        ///     The vector width is defined as the number of scalar elements that can be stored in the vector.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        NativeVectorWidthChar = 0x1036,

        /// <inheritdoc cref="NativeVectorWidthChar"/>
        NativeVectorWidthShort = 0x1037,

        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        NativeVectorWidthInt = 0x1038,

        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        NativeVectorWidthLong = 0x1039,

        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        NativeVectorWidthFloat = 0x103A,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.1.</pre></i>
        /// </para>
        /// <para>
        ///     Returns the native ISA vector width.
        ///     The vector width is defined as the number of scalar elements that can be stored in the vector.
        /// </para>
        /// <para>
        ///     If double precision is not supported, must return 0.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        NativeVectorWidthDouble = 0x103B,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.1.</pre></i>
        /// </para>
        /// <para>
        ///     Returns the native ISA vector width.
        ///     The vector width is defined as the number of scalar elements that can be stored in the vector.
        /// </para>
        /// <para>
        ///     If the <c>cl_khr_fp16</c> extension is not supported, must return 0.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        NativeVectorWidthHalf = 0x103C,

        /// <summary>
        /// <list type="bullet">
        /// <item>
        ///     <term><b>Before OpenCL 2.2: </b></term>
        ///     <description>Maximum configured clock frequency of the device in MHz.</description>
        /// </item>
        ///
        /// <item>
        ///     <term><b>After OpenCL 2.2: </b></term>
        ///     <description>Clock frequency of the device in MHz.
        ///     The meaning of this value is implementation-defined.
        ///     For devices with multiple clock domains,
        ///     the clock frequency for any of the clock domains may be returned.
        ///     For devices that dynamically change frequency for power or thermal reasons,
        ///     the returned clock frequency may be any valid frequency.</description>
        /// </item>
        /// </list>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        MaximumClockFrequency = 0x100C,

        /// <summary>
        /// <para>
        ///     The default compute device address space size of the global
        ///     address space specified as an unsigned integer value in bits.
        ///     Currently supported values are 32 or 64 bits.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        AddressBits = 0x100D,

        /// <summary>
        /// <para>
        ///     Max size of memory object allocation in bytes.
        ///     The minimum value is:
        /// </para>
        /// <para>
        /// <pre>max(min(1024 × 1024 × 1024, 1/4th of <see cref="GlobalMemorySize">DeviceInfo.GlobalMemorySize</see>), 32 × 1024 × 1024)</pre>
        /// </para>
        /// <para>
        ///     for devices that are not of type <c><see cref="DeviceType.Custom">DeviceType.Custom</see></c>.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>ulong</c>
        /// </para>
        /// </summary>
        MaximumMemoryAllocationSize = 0x1010,

        /// <summary>
        /// <para>
        ///     Is TRUE if images are supported by the OpenCL device and FALSE otherwise.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>bool</c>
        /// </para>
        /// </summary>
        ImageSupport = 0x1016,

        /// <summary>
        /// <para>
        ///     Max number of image objects arguments of a kernel
        ///     declared with the read_only qualifier.
        ///     The minimum value is 128 if <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE, the value is 0 otherwise.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        MaximumReadImageArguments = 0x100E,

        /// <summary>
        /// <para>
        ///     Max number of image objects arguments of a kernel
        ///     declared with the write_only qualifier.
        ///     The minimum value is 64 if <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE, the value is 0 otherwise.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        MaximumWriteImageArguments = 0x100F,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     Max number of image objects arguments of a kernel
        ///     declared with the <c>write_only</c> or <c>read_write</c> qualifier.
        /// </para>
        /// <para>
        ///     The minimum value is 64 if the device supports read-write images arguments,
        ///     and must be 0 for devices that do not support read-write images.
        /// </para>
        /// <para>
        ///     Support for read-write image arguments is required for
        ///     an OpenCL 2.0, 2.1, or 2.2 device if <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        MaximumReadWriteImageArguments = 0x104C,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.1.</pre></i>
        /// </para>
        /// <para>
        ///     The intermediate languages that can be supported by
        ///     <c><see cref="CL.CreateProgramWithIL(CLContext, IntPtr, UIntPtr, out CLResultCode)">CreateProgramWithIL()</see></c>
        ///     for this device. Returns a space-separated list of IL version strings of the form
        ///     <c>&lt;IL_Prefix&gt;_&lt;Major_Version&gt;.&lt;Minor_Version&gt;</c>.
        /// </para>
        /// <para>
        ///      For an OpenCL 2.1 or 2.2 device, SPIR-V is a required IL prefix.
        ///      If the device does not support intermediate language programs, the value must be "" (an empty string).
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>string</c>
        /// </para>
        /// </summary>
        IntermediateLanguageVersion = 0x105B,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 3.0.</pre></i>
        /// </para>
        /// <para>
        ///     Returns an array of descriptions (name and version)
        ///     for all supported intermediate languages.
        ///     Intermediate languages with the same name may
        ///     be reported more than once but each name and major/minor
        ///     version combination may only be reported once.
        ///     The list of intermediate languages reported must match
        ///     the list reported via <c><see cref="IntermediateLanguageVersion">IntermediateLanguageVersion</see></c>.
        /// </para>
        /// <para>
        ///      For an OpenCL 2.1 or 2.2 device, at least one version of SPIR-V must be reported.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>cl_name_version[]</c>
        /// </para>
        /// </summary>
        IntermediateLanguagesWithVersion = 0x1061,

        /// <summary>
        /// <para>
        ///     Max width of 2D image or 1D image not created from a buffer object in pixels.
        ///     The minimum value is 16384 if <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE,
        ///     the value is 0 otherwise.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>UIntPtr</c>
        /// </para>
        /// </summary>
        Image2DMaximumWidth = 0x1011,

        /// <summary>
        /// <para>
        ///     Max height of 2D image in pixels.
        ///     The minimum value is 16384 if <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE,
        ///     the value is 0 otherwise.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>UIntPtr</c>
        /// </para>
        /// </summary>
        Image2DMaximumHeight = 0x1012,

        /// <summary>
        /// <para>
        ///     Max width of 3D image in pixels.
        ///     The minimum value is 2048 if <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE,
        ///     the value is 0 otherwise.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>UIntPtr</c>
        /// </para>
        /// </summary>
        Image3DMaximumWidth = 0x1013,

        /// <summary>
        /// <para>
        ///     Max height of 3D image in pixels.
        ///     The minimum value is 2048 if <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE,
        ///     the value is 0 otherwise.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>UIntPtr</c>
        /// </para>
        /// </summary>
        Image3DMaximumHeight = 0x1014,

        /// <summary>
        /// <para>
        ///     Max depth of 3D image in pixels.
        ///     The minimum value is 2048 if <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE,
        ///     the value is 0 otherwise.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>UIntPtr</c>
        /// </para>
        /// </summary>
        Image3DMaximumDepth = 0x1015,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.2.</pre></i>
        /// </para>
        /// <para>
        ///     Max number of pixels for a 1D image created from a buffer object.
        ///     The minimum value is 65536 if <c><see cref="ImageSupport">ImageSupport</see></c>
        ///     is TRUE, the value is 0 otherwise.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>UIntPtr</c>
        /// </para>
        /// </summary>
        ImageMaximumBufferSize = 0x1040,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.2.</pre></i>
        /// </para>
        /// <para>
        ///     Max number of images in a 1D or 2D image array.
        ///     The minimum value is 2048 <c><see cref="ImageSupport">ImageSupport</see></c>
        ///     is TRUE, the value is 0 otherwise.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>UIntPtr</c>
        /// </para>
        /// </summary>
        ImageMaximumArraySize = 0x1041,

        /// <summary>
        /// <para>
        ///     Maximum number of samplers that can be used in a kernel.
        ///     The minimum value is 16 if <c><see cref="ImageSupport">ImageSupport</see></c>
        ///     is TRUE, the value is 0 otherwise.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        MaximumSamplers = 0x1018,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     The row pitch alignment size in pixels for 2D images
        ///     created from a buffer. The value returned must be a power of 2.
        /// </para>
        /// <para>
        ///     Support for 2D images created from a buffer is required
        ///     for an OpenCL 2.0, 2.1, or 2.2 device if <c><see cref="ImageSupport">ImageSupport</see></c>
        ///     is TRUE.
        /// </para>
        /// <para>
        ///     This value must be 0 for devices that do not support 2D images created from a buffer.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        ImagePitchAlignment = 0x104A,

        /// #TODO: CreateBufferWithProperties() isnt implemented
        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     This query specifies the minimum alignment in pixels of the <c>host_ptr</c> specified to
        ///     <c><see cref="CL.CreateBuffer(CLContext, MemoryFlags, UIntPtr, IntPtr, out CLResultCode)">CreateBuffer()</see></c> or
        ///     <c><see cref="CL.CreateBufferWithProperties()">CreateBufferWithProperties()</see></c>
        ///     when a 2D image is created from a buffer which was created using <c><see cref="MemoryFlags.UseHostPtr">UseHostPtr</see></c>.
        ///     The value returned must be a power of 2.
        /// </para>
        /// <para>
        ///     Support for 2D images created from a buffer is required
        ///     for an OpenCL 2.0, 2.1, or 2.2 device if <c><see cref="ImageSupport">ImageSupport</see></c>
        ///     is TRUE.
        /// </para>
        /// <para>
        ///     This value must be 0 for devices that do not support 2D images created from a buffer.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        ImageBaseAddressAlignment = 0x104B,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     The maximum number of pipe objects that can be passed
        ///     as arguments to a kernel. The minimum value is 16 for devices supporting pipes,
        ///     and must be 0 for devices that do not support pipes.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        MaximumPipeArguments = 0x1055,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     The maximum number of reservations that can be active for a pipe
        ///     per work-item in a kernel. A work-group reservation is counted as one
        ///     reservation per work-item. The minimum value is 1 for devices supporting pipes,
        ///     and must be 0 for devices that do not support pipes.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        PipeMaximumActiveReservations = 0x1056,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     The maximum size of pipe packet in bytes.
        /// </para>
        /// <para>
        ///     Support for pipes is required for an OpenCL 2.0, 2.1, or 2.2 device.
        ///     The minimum value is 1024 bytes if the device supports pipes,
        ///     and must be 0 for devices that do not support pipes.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        PipeMaximumPacketSize = 0x1057,

        /// <summary>
        /// <para>
        ///     Max size in bytes of all arguments that can be passed to a kernel.
        ///     The minimum value is 1024 for devices that are not of type
        ///     <c><see cref="DeviceType.Custom">DeviceType.Custom</see></c>.
        ///     For this minimum value, only a maximum of 128 arguments can be passed to a kernel.
        ///     For all other values, a maximum of 255 arguments can be passed to a kernel.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>UIntPtr</c>
        /// </para>
        /// </summary>
        MaximumParameterSize = 0x1017,

        /// <summary>
        /// <para>
        ///     Alignment requirement (in bits) for sub-buffer offsets.
        ///     The minimum value is the size (in bits) of the largest OpenCL built-in data type
        ///     supported by the device (long16 in FULL profile, long16 or int16 in EMBEDDED profile)
        ///     for devices that are not of type <c><see cref="DeviceType.Custom">DeviceType.Custom</see></c>.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        MemoryBaseAddressAlignment = 0x1019,

        /// <summary>
        /// <para>
        ///     <i><pre>Deprecated by version 1.2.</pre></i>
        /// </para>
        /// <para>
        ///     The minimum value is the size (in bytes)
        ///     of the largest OpenCL data type supported by the device
        ///     (long16 in FULL profile, long16 or int16 in EMBEDDED profile).
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        [Obsolete("MinimumDataTypeAlignmentSize is a deprecated OpenCL 1.1 property.")]
        MinimumDataTypeAlignmentSize = 0x101A,

        /// #TODO: Missing return type equivalent in the API
        /// <summary>
        /// <para>
        ///     Describes single precision floating-point capability of the device.
        /// </para>
        /// <para>
        ///     For the full profile, the mandated minimum floating-point capability
        ///     for devices that are not of type <c><see cref="DeviceType.Custom">Custom</see></c> is:
        /// </para>
        /// <para>
        ///     <pre><see cref="DeviceFloatingPointConfig.RoundToNearest">RoundToNearest</see> | <see cref="DeviceFloatingPointConfig.InfinityNaN">InfinityNaN</see></pre>
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="DeviceFloatingPointConfig">DeviceFloatingPointConfig</see></c>
        /// </para>
        /// </summary>
        SingleFloatingPointConfiguration = 0x101B,

        /// #TODO: Missing return type equivalent in the API
        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.2.</pre></i>
        /// </para>
        /// <para>
        ///     Describes double precision floating-point capability of the OpenCL device.
        ///     Double precision is an optional feature so the mandated minimum double
        ///     precision floating-point capability is 0.
        /// </para>
        /// <para>
        ///     If double precision is supported by the device,
        ///     then the minimum double precision floating-point capability
        ///     for OpenCL 2.0 or newer devices is:
        /// </para>
        /// <para>
        ///     <pre><see cref="DeviceFloatingPointConfig.FusedMultiplyAdd">FusedMultiplyAdd</see> | <see cref="DeviceFloatingPointConfig.RoundToNearest">RoundToNearest</see> | <see cref="DeviceFloatingPointConfig.InfinityNaN">InfinityNaN</see> | <see cref="DeviceFloatingPointConfig.Denorm">Denorm</see></pre>
        /// </para>
        /// <para>
        ///     or for OpenCL 1.0, OpenCL 1.1 or OpenCL 1.2 devices:
        /// </para>
        /// <para>
        ///     <pre><see cref="DeviceFloatingPointConfig.FusedMultiplyAdd">FusedMultiplyAdd</see> | <see cref="DeviceFloatingPointConfig.RoundToNearest">RoundToNearest</see> | <see cref="DeviceFloatingPointConfig.RoundToZero">RoundToZero</see> | <see cref="DeviceFloatingPointConfig.RoundToInfinity">RoundToInfinity</see> | <see cref="DeviceFloatingPointConfig.InfinityNaN">InfinityNaN</see> | <see cref="DeviceFloatingPointConfig.Denorm">Denorm</see></pre>
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="DeviceFloatingPointConfig">DeviceFloatingPointConfig</see></c>
        /// </para>
        /// </summary>
        DoubleFloatingPointConfiguration = 0x1032,

        /// #TODO: Missing return type equivalent in the API
        /// <summary>
        /// <para>
        ///     Type of global memory cache supported.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="DeviceMemoryCacheType">DeviceMemoryCacheType</see></c>
        /// </para>
        /// </summary>
        GlobalMemoryCacheType = 0x101C,

        /// <summary>
        /// <para>
        ///     Size of global memory cache line in bytes.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        GlobalMemoryCachelineSize = 0x101D,

        /// <summary>
        /// <para>
        ///     Size of global memory cache in bytes.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>ulong</c>
        /// </para>
        /// </summary>
        GlobalMemoryCacheSize = 0x101E,

        /// <summary>
        /// <para>
        ///     Size of global device memory in bytes.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>ulong</c>
        /// </para>
        /// </summary>
        GlobalMemorySize = 0x101F,

        /// <summary>
        /// <para>
        ///     Max size in bytes of a constant buffer allocation.
        ///     The minimum value is 64 KB for devices that are not of type
        ///     <c><see cref="DeviceType.Custom">Custom</see></c>.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>ulong</c>
        /// </para>
        /// </summary>
        MaximumConstantBufferSize = 0x1020,

        /// <summary>
        /// <para>
        ///     Max number of arguments declared with the <c>__constant</c> qualifier in a kernel.
        ///     The minimum value is 8 for devices that are not of type
        ///     <c><see cref="DeviceType.Custom">Custom</see></c>.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        MaximumConstantArguments = 0x1021,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     The maximum number of bytes of storage that may be allocated for any single
        ///     variable in program scope or inside a function in an OpenCL kernel
        ///     language declared in the global address space.
        /// </para>
        /// <para>
        ///     Support for program scope global variables is required
        ///     for an OpenCL 2.0, 2.1, or 2.2 device. The minimum value is 64 KB if the device
        ///     supports program scope global variables, and must be 0 for devices that do
        ///     not support program scope global variables.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>UIntPtr</c>
        /// </para>
        /// </summary>
        MaximumGlobalVariableSize = 0x104D,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     Maximum preferred total size, in bytes,
        ///     of all program variables in the global address space.
        ///     This is a performance hint. An implementation may place such variables
        ///     in storage with optimized device access.
        ///     This query returns the capacity of such storage. The minimum value is 0.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>UIntPtr</c>
        /// </para>
        /// </summary>
        GlobalVariablePreferredTotalSize = 0x1054,

        /// #TODO: Missing return type equivalent in the API
        /// <summary>
        /// <para>
        ///     Type of local memory supported.
        ///     This can be set to <c><see cref="DeviceLocalMemoryType.Local">Local</see></c>
        ///     implying dedicated local memory storage such as SRAM, or
        ///     <c><see cref="DeviceLocalMemoryType.Global">Global</see></c>.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="DeviceLocalMemoryType">DeviceLocalMemoryType</see></c>
        /// </para>
        /// </summary>
        LocalMemoryType = 0x1022,

        /// <summary>
        /// <para>
        ///     Size of local memory arena in bytes.
        ///     The minimum value is 32 KB for devices that are not of type
        ///     <c><see cref="DeviceType.Custom">Custom</see></c>..
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>ulong</c>
        /// </para>
        /// </summary>
        LocalMemorySize = 0x1023,

        /// <summary>
        /// <para>
        ///     Is TRUE if the device implements error correction
        ///     for all accesses to compute device memory (global and constant).
        ///     Is FALSE if the device does not implement such error correction.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>bool</c>
        /// </para>
        /// </summary>
        ErrorCorrectionSupport = 0x1024,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.1 and deprecated by version 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     Is TRUE if the device and the host have a unified memory subsystem and is FALSE otherwise.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>bool</c>
        /// </para>
        /// </summary>
        [Obsolete("HostUnifiedMemory is a deprecated OpenCL 1.2 property.")]
        HostUnifiedMemory = 0x1035,

        /// <summary>
        /// <para>
        ///     Describes the resolution of device timer. This is measured in nanoseconds. Refer to
        ///     <see href="https://www.khronos.org/registry/OpenCL/specs/3.0-unified/html/OpenCL_API.html#profiling-operations">Profiling Operations</see> for details.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>UIntPtr</c>
        /// </para>
        /// </summary>
        ProfilingTimerResolution = 0x1025,

        /// <summary>
        /// <para>
        ///     Is TRUE if the OpenCL device is a little endian device and FALSE otherwise.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>bool</c>
        /// </para>
        /// </summary>
        EndianLittle = 0x1026,

        /// <summary>
        /// <para>
        ///     Is TRUE if the device is available and FALSE if the device is not available.
        ///     A device is considered to be available if the device can be expected to
        ///     successfully execute commands enqueued to the device.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>bool</c>
        /// </para>
        /// </summary>
        Available = 0x1027,

        /// <summary>
        /// <para>
        ///     Is FALSE if the implementation does not have
        ///     a compiler available to compile the program source.
        ///     Is TRUE if the compiler is available.
        ///     This can be FALSE for the embedded platform profile only.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>bool</c>
        /// </para>
        /// </summary>
        CompilerAvailable = 0x1028,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.2.</pre></i>
        /// </para>
        /// <para>
        ///     Is FALSE if the implementation does not have a linker available. Is TRUE if the linker is available.
        /// </para>
        /// <para>
        ///     This can be FALSE for the embedded platform profile only.
        ///     This must be TRUE if <c><see cref="CompilerAvailable">CompilerAvailable</see></c> is TRUE.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>bool</c>
        /// </para>
        /// </summary>
        LinkerAvailable = 0x103E,

        /// #TODO: Missing return type equivalent in the API
        /// <summary>
        /// <para>
        ///     Describes the execution capabilities of the device.
        ///     This is a bit-field that describes one or more of the following values:
        /// </para>
        /// <list type="bullet">
        /// <item><term>
        ///     <c><see cref="DeviceExecutionCapabilities.Kernel">Kernel</see></c></term>
        /// <description>
        ///     The OpenCL device can execute OpenCL kernels.
        /// </description></item>
        ///
        /// <item><term>
        ///     <c><see cref="DeviceExecutionCapabilities.NativeKernel">NativeKernel</see></c></term>
        /// <description>
        ///     The OpenCL device can execute native kernels.
        /// </description></item>
        /// </list>
        /// <para>
        ///     The mandated minimum capability is
        ///     <c><see cref="DeviceExecutionCapabilities.Kernel">Kernel</see></c>.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="DeviceExecutionCapabilities">DeviceExecutionCapabilities</see></c>
        /// </para>
        /// </summary>
        ExecutionCapabilities = 0x1029,

        /// #TODO: Unfinished return type in API
        /// <summary>
        /// <para>
        ///     <i><pre>Deprecated by verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     See description of <c><see cref="QueueOnHostProperties">QueueOnHostProperties</see></c>
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="CommandQueueProperty">CommandQueueProperty</see></c>
        /// </para>
        /// </summary>
        [Obsolete("QueueProperties is a deprecated OpenCL 1.2 property, please use QueueOnHostProperties.")]
        QueueProperties = 0x102A,

        /// #TODO: Unfinished return type in API
        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     Describes the on host command-queue properties supported by the device.
        ///     The mandated minimum capability is:
        ///     <c><see cref="CommandQueueProperty.ProfilingEnable">ProfilingEnable</see></c>.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="CommandQueueProperty">CommandQueueProperty</see></c>
        /// </para>
        /// </summary>
        QueueOnHostProperties = 0x102A,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     Describes the on device command-queue properties supported by the device.
        /// </para>
        /// <para>
        ///     Support for on-device queues is required for an OpenCL 2.0, 2.1, or 2.2 device.
        ///     When on-device queues are supported, the mandated minimum capability is:
        /// </para>
        /// <para>
        ///     <pre><see cref="CommandQueueProperty.OutOfOrderExecutionModeEnable">OutOfOrderExecutionModeEnable</see> | <see cref="CommandQueueProperty.ProfilingEnable">ProfilingEnable</see></pre>
        /// </para>
        /// <para>
        ///     Must be 0 for devices that do not support on-device queues.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="CommandQueueProperty">CommandQueueProperty</see></c>
        /// </para>
        /// </summary>
        QueueOnDeviceProperties = 0x104E,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     The preferred size of the device queue, in bytes.
        ///     Applications should use this size for the device queue to ensure good performance.
        /// </para>
        /// <para>
        ///     The minimum value is 16 KB for devices supporting on-device queues,
        ///     and must be 0 for devices that do not support on-device queues.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        QueueOnDevicePreferredSize = 0x104F,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     The maximum size of the device queue in bytes.
        /// </para>
        /// <para>
        ///     The minimum value is 256 KB for the full profile and 64 KB for
        ///     the embedded profile for devices supporting on-device queues,
        ///     and must be 0 for devices that do not support on-device queues.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        QueueOnDeviceMaximumSize = 0x1050,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     The maximum number of device queues that can be created for this device in a single context.
        /// </para>
        /// <para>
        ///     The minimum value is 1 for devices supporting on-device queues,
        ///     and must be 0 for devices that do not support on-device queues.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        MaximumOnDeviceQueues = 0x1051,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///      The maximum number of events in use by a device queue.
        ///      These refer to events returned by the <c>enqueue_</c> built-in functions
        ///      to a device queue or user events returned by the <c>create_user_event</c>
        ///      built-in function that have not been released.
        /// </para>
        /// <para>
        ///     The minimum value is 1024 for devices supporting on-device queues,
        ///     and must be 0 for devices that do not support on-device queues.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        MaximumOnDeviceEvents = 0x1052,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.2.</pre></i>
        /// </para>
        /// <para>
        ///      A semi-colon separated list of built-in kernels supported by the device.
        ///      An empty string is returned if no built-in kernels are supported by the device.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>string</c>
        /// </para>
        /// </summary>
        BuiltInKernels = 0x103F,

        /// #TODO: missing equivalent return type in API
        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 3.0.</pre></i>
        /// </para>
        /// <para>
        ///     Returns an array of descriptions for the built-in kernels supported by the device.
        ///     Each built-in kernel may only be reported once.
        ///     The list of reported kernels must match the list returned via
        ///     <c><see cref="BuiltInKernels">BuiltInKernels</see></c>.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>cl_name_version[]</c>
        /// </para>
        /// </summary>
        BuiltInKernelsWithVersion = 0x1062,

        /// <summary>
        /// <para>
        ///     The platform associated with this device.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="CLPlatform">CLPlatform</see></c>
        /// </para>
        /// </summary>
        Platform = 0x1031,

        /// <summary>
        /// <para>
        ///     Device name string.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>string</c>
        /// </para>
        /// </summary>
        Name = 0x102B,

        /// <summary>
        /// <para>
        ///     Vendor name string.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>string</c>
        /// </para>
        /// </summary>
        /// <remarks>Return Type: string</remarks>
        Vendor = 0x102C,

        /// <summary>
        /// <para>
        ///     OpenCL software driver version string. Follows a vendor-specific format.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>string</c>
        /// </para>
        /// </summary>
        DriverVersion = 0x102D,

        /// <summary>
        /// <para>
        ///     OpenCL profile string.
        ///     Returns the profile name supported by the device.
        ///     The profile name returned can be one of the following strings:
        /// </para>
        /// <list type="bullet">
        /// <item>
        ///     <term><b>FULL_PROFILE: </b></term>
        ///     <description>
        ///         if the device supports the OpenCL specification
        ///         (functionality defined as part of the core specification and
        ///         does not require any extensions to be supported).
        ///     </description>
        /// </item>
        ///
        /// <item>
        ///     <term><b>EMBEDED_PROFILE: </b></term>
        ///     <description>
        ///         if the device supports the OpenCL embedded profile.
        ///     </description>
        /// </item>
        /// </list>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>string</c>
        /// </para>
        /// </summary>
        Profile = 0x102E,

        /// <summary>
        /// <para>
        ///     OpenCL version string. Returns the OpenCL version supported by the device.
        ///     This version string has the following format:
        /// </para>
        /// <para>
        ///     <c><pre>OpenCL&lt;space&gt;&lt;major_version.minor_version&gt;&lt;space&gt;&lt;vendor-specific information&gt;</pre></c>
        /// </para>
        /// <para>
        ///     The <c>major_version.minor_version</c>
        ///     value returned will be one of 1.0, 1.1, 1.2, 2.0, 2.1, 2.2, or 3.0.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>string</c>
        /// </para>
        /// </summary>
        Version = 0x102F,

        /// #TODO: Missing equivalent return type in the API
        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 3.0.</pre></i>
        /// </para>
        /// <para>
        ///     Returns the detailed (major, minor, patch)
        ///     version supported by the device. The major and minor version numbers
        ///     returned must match those returned via <c><see cref="Version">Version</see></c>.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>cl_version</c>
        /// </para>
        /// </summary>
        NumericVersion = 0x105E,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.1 and deprecated by version 3.0.</pre></i>
        /// </para>
        /// <para>
        ///     Returns the highest fully backwards compatible OpenCL C version
        ///     supported by the compiler for the device.
        ///     For devices supporting compilation from OpenCL C source,
        ///     this will return a version string with the following format:
        /// </para>
        /// <para>
        ///     <c><pre>OpenCL&lt;space&gt;C&lt;space&gt;&lt;major_version.minor_version&gt;&lt;space&gt;&lt;vendor-specific information&gt;</pre></c>
        /// </para>
        /// <para>
        ///     For devices that support compilation from OpenCL C source:
        /// </para>
        /// <para>
        ///     Because OpenCL 3.0 is backwards compatible with OpenCL C 1.2,
        ///     an OpenCL 3.0 device must support at least OpenCL C 1.2. An OpenCL 3.0
        ///     device may return an OpenCL C version newer than OpenCL C 1.2 if and only
        ///     if all optional OpenCL C features are supported by the device for the newer version.
        /// </para>
        /// <para>
        ///     Support for OpenCL C 2.0 is required for an OpenCL 2.0, OpenCL 2.1, or OpenCL 2.2 device.
        /// </para>
        /// <para>
        ///     Support for OpenCL C 1.2 is required for an OpenCL 1.2 device.
        /// </para>
        /// <para>
        ///     Support for OpenCL C 1.1 is required for an OpenCL 1.1 device.
        /// </para>
        /// <para>
        ///     Support for either OpenCL C 1.0 or OpenCL C 1.1 is required for an OpenCL 1.0 device.
        /// </para>
        /// <para>
        ///     For devices that do not support compilation from OpenCL C source,
        ///     such as when <c><see cref="CompilerAvailable">CompilerAvailable</see></c>
        ///     is FALSE, this query may return an empty string.
        /// </para>
        /// <para>
        ///     This query has been superseded by the CL_DEVICE_OPENCL_C_ALL_VERSIONS query, which returns a set of OpenCL C versions supported by a device.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>string</c>
        /// </para>
        /// </summary>
        OpenClCVersion = 0x103D,

        /// #TODO: Missing equivalent return type in the API
        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 3.0.</pre></i>
        /// </para>
        /// <para>
        ///     Returns an array of name, version descriptions listing all
        ///     the versions of OpenCL C supported by the compiler for the device.
        ///     In each returned description structure, the name field is required
        ///     to be "OpenCL C". The list may include both newer non-backwards compatible OpenCL C versions,
        ///     such as OpenCL C 3.0, and older OpenCL C versions with mandatory backwards compatibility.
        ///     The version returned by <c><see cref="OpenClCVersion">OpenClCVersion</see></c>
        ///     is required to be present in the list.
        /// </para>
        /// <para>
        ///     For devices that support compilation from OpenCL C source:
        /// </para>
        /// <para>
        ///     Because OpenCL 3.0 is backwards compatible with OpenCL C 1.2,
        ///     and OpenCL C 1.2 is backwards compatible with OpenCL C 1.1 and OpenCL C 1.0,
        ///     support for at least OpenCL C 3.0, OpenCL C 1.2, OpenCL C 1.1,
        ///     and OpenCL C 1.0 is required for an OpenCL 3.0 device.
        /// </para>
        /// <para>
        ///     Support for OpenCL C 2.0, OpenCL C 1.2, OpenCL C 1.1,
        ///     and OpenCL C 1.0 is required for an OpenCL 2.0, OpenCL 2.1,
        ///     or OpenCL 2.2 device.
        /// </para>
        /// <para>
        ///     Support for OpenCL C 1.2, OpenCL C 1.1, and OpenCL C 1.0
        ///     is required for an OpenCL 1.2 device.
        /// </para>
        /// <para>
        ///     Support for OpenCL C 1.1 and OpenCL C 1.0 is required for an OpenCL 1.1 device.
        /// </para>
        /// <para>
        ///     Support for at least OpenCL C 1.0 is required for an OpenCL 1.0 device.
        /// </para>
        /// <para>
        ///    For devices that do not support compilation from OpenCL C source,
        ///    this query may return an empty array. this query may return an empty string.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>cl_name_version[]</c>
        /// </para>
        /// </summary>
        OpenClCAllVersions = 0x1066,

        /// #TODO: Missing equivalent return type in the API
        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 3.0.</pre></i>
        /// </para>
        /// <para>
        ///     Returns an array of optional OpenCL C features supported by the compiler
        ///     for the device alongside the OpenCL C version that introduced the feature macro.
        ///     For example, if a compiler supports an OpenCL C 3.0 feature,
        ///     the returned name will be the full name of the OpenCL C feature macro,
        ///     and the returned version will be 3.0.0.
        /// </para>
        /// <para>
        ///     For devices that do not support compilation from OpenCL C source,
        ///     this query may return an empty array.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>cl_name_version[]</c>
        /// </para>
        /// </summary>
        OpenCLCFeatures = 0x106F,

        /// <summary>
        /// <para>
        ///     Returns a space separated list of extension names
        ///     (the extension names themselves do not contain any spaces) supported by the device.
        ///     The list of extension names may include Khronos
        ///     approved extension names and vendor specified extension names.
        /// </para>
        /// <para>
        ///     The following Khronos extension names must be returned by all devices that support OpenCL 1.1:
        /// </para>
        /// <list type="bullet">
        /// <item>
        ///     <description><b>cl_khr_byte_addressable_store</b></description>
        /// </item>
        /// <item>
        ///     <description><b>cl_khr_global_int32_base_atomics</b></description>
        /// </item>
        /// <item>
        ///     <description><b>cl_khr_global_int32_extended_atomics</b></description>
        /// </item>
        /// <item>
        ///     <description><b>cl_khr_local_int32_base_atomics</b></description>
        /// </item>
        /// <item>
        ///     <description><b>cl_khr_local_int32_extended_atomics</b></description>
        /// </item>
        /// </list>
        /// <para>
        ///     Additionally, the following Khronos extension names must be returned by all devices
        ///     that support OpenCL 1.2 when and only when the optional feature is supported:
        /// </para>
        /// <list type="bullet">
        /// <item>
        ///     <description><b>cl_khr_fp64</b></description>
        /// </item>
        /// </list>
        /// <para>
        ///     Additionally, the following Khronos extension names must be returned
        ///     by all devices that support OpenCL 2.0, OpenCL 2.1, or OpenCL 2.2.
        ///     For devices that support OpenCL 3.0,
        ///     these extension names must be returned when and only when the optional feature is supported:
        /// </para>
        /// <list type="bullet">
        /// <item>
        ///     <description><b>cl_khr_3d_image_writes</b></description>
        /// </item>
        /// <item>
        ///     <description><b>cl_khr_depth_images</b></description>
        /// </item>
        /// <item>
        ///     <description><b>cl_khr_image2d_from_buffer</b></description>
        /// </item>
        /// </list>
        /// <para>
        ///     Please refer to the OpenCL Extension Specification or vendor provided documentation
        ///     for a detailed description of these extensions.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>string</c>
        /// </para>
        /// </summary>
        Extensions = 0x1030,

        /// #TODO: Missing equivalent return type in the API
        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 3.0.</pre></i>
        /// </para>
        /// <para>
        ///     Returns an array of description (name and version) structures.
        ///     The same extension name must not be reported more than once.
        ///     The list of extensions reported must match the list reported via
        ///     <c><see cref="Extensions">Extensions</see></c>.
        /// </para>
        /// <para>
        ///     See <c><see cref="Extensions">Extensions</see></c>
        ///     for a list of extensions that are required to be reported for a given OpenCL version.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>cl_name_version[]</c>
        /// </para>
        /// </summary>
        ExtensionsWithVersion = 0x1060,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.2.</pre></i>
        /// </para>
        /// <para>
        ///     Maximum size in bytes of the internal buffer that holds the output of
        ///     printf calls from a kernel. The minimum value for the FULL profile is 1 MB.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>UIntPtr</c>
        /// </para>
        /// </summary>
        PrintfBufferSize = 0x1049,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.2.</pre></i>
        /// </para>
        /// <para>
        ///     Is TRUE if the devices preference is for the user to be responsible
        ///     for synchronization, when sharing memory objects between OpenCL and other APIs
        ///     such as DirectX, FALSE if the device / implementation has a performant path
        ///     for performing synchronization of memory object shared between
        ///     OpenCL and other APIs such as DirectX.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>bool</c>
        /// </para>
        /// </summary>
        PreferredInteropUserSync = 0x1048,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.2.</pre></i>
        /// </para>
        /// <para>
        ///     Returns the <c><see cref="CLDevice">CLDevice</see></c> of the parent
        ///     device to which this sub-device belongs.
        ///     If <c><see cref="CL.GetDeviceInfo(CLDevice, DeviceInfo, UIntPtr, byte[], out UIntPtr)">device</see></c> is a root-level device, a NULL value is returned.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="CLDevice">CLDevice</see></c>
        /// </para>
        /// </summary>
        ParentDevice = 0x1042,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.2.</pre></i>
        /// </para>
        /// <para>
        ///     Returns the maximum number of sub-devices that can be created when a device is partitioned.
        ///     The value returned cannot exceed <c><see cref="MaximumComputeUnits">MaximumComputeUnits</see></c>.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        PartitionMaximumSubDevices = 0x1043,

        /// #TODO: Missing return type equivalent in the API
        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.2.</pre></i>
        /// </para>
        /// <para>
        ///     Returns the list of partition types supported by device.
        ///     If the device cannot be partitioned (i.e. there is no partitioning scheme
        ///     supported by the device that will return at least two sub-devices),
        ///     a value of 0 will be returned.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="DevicePartitionProperty">DevicePartitionProperty[]</see></c>
        /// </para>
        /// </summary>
        PartitionProperties = 0x1044,

        /// #TODO: Missing return type equivalent in the API
        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.2.</pre></i>
        /// </para>
        /// <para>
        ///     Returns the list of supported affinity domains for
        ///     partitioning the device using
        ///     <c><see cref="DevicePartitionProperty.ByAffinityDomain">ByAffinityDomain</see></c>.
        /// </para>
        /// <para>
        ///     If the device does not support any affinity domains, a value of 0 will be returned.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="DeviceAffinityDomain">DeviceAffinityDomain</see></c>
        /// </para>
        /// </summary>
        PartitionAffinityDomain = 0x1045,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.2.</pre></i>
        /// </para>
        /// <para>
        ///     Returns the properties argument specified in
        ///     <c><see cref="CL.CreateSubDevices(CLDevice, IntPtr[], uint, CLDevice[], out uint)">CreateSubDevices()</see></c>
        ///     if device is a sub-device. In the case where the properties argument to
        ///     <c><see cref="CL.CreateSubDevices(CLDevice, IntPtr[], uint, CLDevice[], out uint)">CreateSubDevices()</see></c>
        ///     is <c><see cref="DevicePartitionProperty.ByAffinityDomain">ByAffinityDomain</see></c>,
        ///     C<c><see cref="DeviceAffinityDomain.NextPartionable">NextPartionable</see></c>,
        ///     the affinity domain used to perform the partition will be returned.
        /// </para>
        /// <para>
        ///     Otherwise the implementation may either return a
        ///     <c><see cref="CL.GetDeviceInfo(CLDevice, DeviceInfo, UIntPtr, byte[], out UIntPtr)">paramValueSizeReturned</see></c>
        ///     of 0 i.e. there is no partition type associated with device or can return
        ///     a property value of 0 (where 0 is used to terminate the partition property list) in the memory that
        ///     <c><see cref="CL.GetDeviceInfo(CLDevice, DeviceInfo, UIntPtr, byte[], out UIntPtr)">paramValue</see></c>
        ///     points to.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="DevicePartitionProperty">DevicePartitionProperty[]</see></c>
        /// </para>
        /// </summary>
        PartitionType = 0x1046,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 1.2.</pre></i>
        /// </para>
        /// <para>
        ///     Returns the <c><see cref="CL.GetDeviceInfo(CLDevice, DeviceInfo, UIntPtr, byte[], out UIntPtr)">device</see></c>
        ///     reference count. If the device is a root-level device, a reference count of one is returned.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        ReferenceCount = 0x1047,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     Describes the various shared virtual memory (SVM) memory allocation types the device supports.
        /// </para>
        /// <para>
        ///      The mandated minimum capability for an OpenCL 2.0, 2.1, or 2.2 device is
        ///      <c><see cref="DeviceSvmCapabilities.CoarseGrainBuffer">CoarseGrainBuffer</see></c>.
        ///      For other device versions there is no mandated minimum capability.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="DeviceSvmCapabilities">DeviceSvmCapabilities</see></c>
        /// </para>
        /// </summary>
        SvmCapabilities = 0x1053,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     Returns the value representing the preferred alignment in bytes for OpenCL 2.0
        ///     fine-grained SVM atomic types. This query can return 0 which indicates that
        ///     the preferred alignment is aligned to the natural size of the type.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        PreferredPlatformAtomicAlignment = 0x1058,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     Returns the value representing the preferred alignment in bytes
        ///     for OpenCL 2.0 atomic types to global memory. This query can return
        ///     0 which indicates that the preferred alignment is aligned to the natural size of the type.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        PreferredGlobalAtomicAlignment = 0x1059,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.0.</pre></i>
        /// </para>
        /// <para>
        ///     Returns the value representing the preferred alignment in
        ///     bytes for OpenCL 2.0 atomic types to local memory. This query
        ///     can return 0 which indicates that the preferred alignment
        ///     is aligned to the natural size of the type.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        PreferredLocalAtomicAlignment = 0x105A,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.1.</pre></i>
        /// </para>
        /// <para>
        ///     Maximum number of sub-groups in a work-group that a device is capable
        ///     of executing on a single compute unit,
        ///     for any given kernel-instance running on the device.
        /// </para>
        /// <para>
        ///     The minimum value is 1 if the device supports sub-groups,
        ///     and must be 0 for devices that do not support sub-groups.
        ///     Support for sub-groups is required for an OpenCL 2.1 or 2.2 device.
        /// </para>
        /// <para>
        ///     (Refer also to
        ///     <c><see cref="CL.GetKernelSubGroupInfo(CLKernel, CLDevice, KernelSubGroupInfo, UIntPtr, IntPtr, UIntPtr, byte[], out UIntPtr)">GetKernelSubGroupInfo()</see></c>.)
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>uint</c>
        /// </para>
        /// </summary>
        MaximumNumberOfSubGroups = 0x105C,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 2.1.</pre></i>
        /// </para>
        /// <para>
        ///     Is TRUE if this device supports independent forward progress of sub-groups, FALSE otherwise.
        /// </para>
        /// <para>
        ///     This query must return TRUE for devices that support the <c>cl_khr_subgroups</c> extension,
        ///     and must return FALSE for devices that do not support sub-groups.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>bool</c>
        /// </para>
        /// </summary>
        SubGroupIndependentForwardProgress = 0x105D,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 3.0.</pre></i>
        /// </para>
        /// <para>
        ///     Describes the various memory orders and scopes that the device supports for atomic memory operations.
        /// </para>
        /// <para>
        ///     Because atomic memory orders are hierarchical,
        ///     a device that supports a strong memory order must also support all weaker memory orders.
        /// </para>
        /// <para>
        ///     Because atomic scopes are hierarchical,
        ///     a device that supports a wide scope must also support all narrower scopes,
        ///     except for the work-item scope, which is a special case.
        /// </para>
        /// <para>
        ///     The mandated minimum capability is:
        /// </para>
        ///     <pre><c><see cref="DeviceAtomicCapabilities.OrderRelaxed">OrderRelaxed</see></c> | <c><see cref="DeviceAtomicCapabilities.ScopeWorkGroup">ScopeWorkGroup</see></c></pre>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="DeviceAtomicCapabilities">DeviceAtomicCapabilities</see></c>
        /// </para>
        /// </summary>
        AtomicMemoryCapabilities = 0x1063,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 3.0.</pre></i>
        /// </para>
        /// <para>
        ///     Describes the various memory orders and scopes that the device
        ///     supports for atomic fence operations. This is a bit-field that has
        ///     the same set of possible values as described for
        ///     <c><see cref="AtomicMemoryCapabilities">AtomicMemoryCapabilities</see></c>.
        /// </para>
        /// <para>
        ///     The mandated minimum capability is:
        /// </para>
        ///     <pre><c><see cref="DeviceAtomicCapabilities.OrderRelaxed">OrderRelaxed</see></c> | <c><see cref="DeviceAtomicCapabilities.OrderAcquireRelease">OrderAcquireRelease</see></c> | <c><see cref="DeviceAtomicCapabilities.ScopeWorkGroup">ScopeWorkGroup</see></c></pre>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c><see cref="DeviceAtomicCapabilities">DeviceAtomicCapabilities</see></c>
        /// </para>
        /// </summary>
        AtomicFenceCapabilities = 0x1064,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 3.0.</pre></i>
        /// </para>
        /// <para>
        ///     Is TRUE if the device supports non-uniform work-groups, and FALSE otherwise.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>bool</c>
        /// </para>
        /// </summary>
        NonUniformGroupSupport = 0x1065,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 3.0.</pre></i>
        /// </para>
        /// <para>
        ///     Is TRUE if the device supports work-group collective functions, e.g.
        ///     <c><see href="https://registry.khronos.org/OpenCL/sdk/3.0/docs/man/html/workGroupFunctions.html">these</see></c>,
        ///     and FALSE otherwise.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>bool</c>
        /// </para>
        /// </summary>
        WorkGroupCollectiveFunctionsSupport = 0x1068,

        /// <summary>
        /// <para>
        ///     <i><pre>Missing before verison 3.0.</pre></i>
        /// </para>
        /// <para>
        ///     Is TRUE if the device supports the generic address space
        ///     and its associated built-in functions, and FALSE otherwise.
        /// </para>
        /// <para>
        ///     <i><u>Return Type:</u></i> <c>bool</c>
        /// </para>
        /// </summary>
        GenericAddressSpaceSupport = 0x1069,

        /// <summary>
        /// 
        /// </summary>
        DeviceEnqueueCapabilities = 0x1070,
        /// <summary>
        /// 
        /// </summary>
        TerminateCapabilityKhr = 0x2031,
        SpirVersion = 0x40E0
    }

    #endregion

    public enum CommandQueueInfo : uint
    {
        Context = 0x1090,
        Device = 0x1091,
        ReferenceCount = 0x1092,
        Properties = 0x1093,
        Size = 0x1094,
        DeviceDefault = 0x1095
    }

    [Flags]
    public enum MemoryFlags : ulong
    {
        ReadWrite = 1 << 0,
        WriteOnly = 1 << 1,
        ReadOnly = 1 << 2,
        UseHostPtr = 1 << 3,
        AllocHostPtr = 1 << 4,
        CopyHostPtr = 1 << 5,

        // Reserved = 1 << 6,
        HostWriteOnly = 1 << 7,
        HostReadOnly = 1 << 8,
        HostNoAccess = 1 << 9,
        SvmFineGrainBuffer = 1 << 10,
        SvmAtomics = 1 << 11,
        KernelReadAndWrite = 1 << 12,

        NoAccessIntel = 1 << 24,
        AccessFlagsUnrestrictedIntel = 1 << 25,
        UseUncachedCpuMemoryImg = 1 << 26,
        UseCachedCpuMemoryImg = 1 << 27,
        UseGrallocPtrImg = 1 << 28,
        ExtHostPtrQcom = 1 << 29,

        // Unused
        //  BusAddressableAmd = 1 << 30,
        // Unused
        //  ExternalMemoryAmd = 1 << 31,

        CL_MEM_RESERVED0_ARM = 1 << 32,
        CL_MEM_RESERVED1_ARM = 1 << 33,
        CL_MEM_RESERVED2_ARM = 1 << 34,
        CL_MEM_RESERVED3_ARM = 1 << 35,
        CL_MEM_RESERVED4_ARM = 1 << 36,
    }

    public enum BufferCreateType : uint
    {
        Region = 0x1220
    }

    public enum MemoryObjectType : uint
    {
        Buffer = 0x10F0,
        Image2D = 0x10F1,
        Image3D = 0x10F2,
        Image2DArray = 0x10F3,
        Image1D = 0x10F4,
        Image1DArray = 0x10F5,
        Image1DBuffer = 0x10F6,
        Pipe = 0x10F7
    }

    public enum ImageInfo : uint
    {
        Format = 0x1110,
        ElementSize = 0x1111,
        RowPitch = 0x1112,
        SlicePitch = 0x1113,
        Width = 0x1114,
        Height = 0x1115,
        Depth = 0x1116,
        ArraySize = 0x1117,
        Buffer = 0x1118,
        NumberOfMipLevels = 0x1119,
        NumberOfSamples = 0x111A
    }

    public enum ChannelOrder : uint
    {
        R = 0x10B0,
        A = 0x10B1,
        Rg = 0x10B2,
        Ra = 0x10B3,
        Rgb = 0x10B4,
        Rgba = 0x10B5,
        Bgra = 0x10B6,
        Argb = 0x10B7,
        Intensity = 0x10B8,
        Luminance = 0x10B9,
        Rx = 0x10BA,
        Rgx = 0x10BB,
        Rgbx = 0x10BC,
        Depth = 0x10BD,
        DepthStencil = 0x10BE,
        Srgb = 0x10BF,
        Srgbx = 0x10C0,
        Srgba = 0x10C1,
        Sbgra = 0x10C2,
        Abgr = 0x10C3
    }

    public enum ChannelType : uint
    {
        NormalizedSignedInteger8 = 0x10D0,
        NormalizedSignedInteger16 = 0x10D1,
        NormalizedUnsignedInteger8 = 0x10D2,
        NormalizedUnsignedInteger16 = 0x10D3,
        NormalizedUnsignedShortFloat565 = 0x10D4,
        NormalizedUnsignedShortFloat555 = 0x10D5,
        NormalizedUnsignedInteger101010 = 0x10D6,
        SignedInteger8 = 0x10D7,
        SignedInteger16 = 0x10D8,
        SignedInteger32 = 0x10D9,
        UnsignedInteger8 = 0x10DA,
        UnsignedInteger16 = 0x10DB,
        UnsignedInteger32 = 0x10DC,
        HalfFloat = 0x10DD,
        Float = 0x10DE,
        NormalizedUnsignedInteger24 = 0x10DF,
        NormalizedUnsignedInteger101010Version2 = 0x10E0
    }

    public enum MemoryObjectInfo : uint
    {
        Type = 0x1100,
        Flags = 0x1101,
        Size = 0x1102,
        HostPointer = 0x1103,
        MapCount = 0x1104,
        ReferenceCount = 0x1105,
        Context = 0x1106,
        AssociatedMemoryObject = 0x1107,
        Offset = 0x1108,
        UsesSvmPointer = 0x1109
    }

    public enum PipeInfo : uint
    {
        PacketSize = 0x1120,
        MaximumNumberOfPackets = 0x1121
    }

    [Flags]
    public enum SvmMemoryFlags : ulong
    {
        ReadWrite = 1 << 0,
        WriteOnly = 1 << 1,
        ReadOnly = 1 << 2,
        UseHostPointer = 1 << 3,
        AllocateHostPointer = 1 << 4,
        CopyHostPointer = 1 << 5,
        HostWriteOnly = 1 << 7,
        HostReadOnly = 1 << 8,
        HostNoAccess = 1 << 9,
        SvmFineGrainBuffer = 1 << 10,
        SvmAtomics = 1 << 11,
        KernelReadAndWrite = 1 << 12
    }

    public enum SamplerInfo : uint
    {
        ReferenceCount = 0x1150,
        Context = 0x1151,
        NormalizedCoordinates = 0x1152,
        AddressingMode = 0x1153,
        FilterMode = 0x1154,
        MipFilterMode = 0x1155,
        LodMinimum = 0x1156,
        LodMaximum = 0x1157
    }

    public enum FilterMode : uint
    {
        Nearest = 0x1140,
        Linear = 0x1141
    }

    public enum AddressingMode : uint
    {
        None = 0x1130,
        ClampToEdge = 0x1131,
        Clamp = 0x1132,
        Repeat = 0x1133,
        MirroredRepeat = 0x1134
    }

    public enum ProgramInfo : uint
    {
        ReferenceCount = 0x1160,
        Context = 0x1161,
        NumberOfDevices = 0x1162,
        Devices = 0x1163,
        Source = 0x1164,
        BinarySizes = 0x1165,
        Binaries = 0x1166,
        NumberOfKernels = 0x1167,
        KernelNames = 0x1168,
        Il = 0x1169
    }

    public enum ProgramBuildInfo : uint
    {
        Status = 0x1181,
        Options = 0x1182,
        Log = 0x1183,
        BinaryType = 0x1184,
        GlobalVariableTotalSize = 0x1185
    }

    public enum KernelExecInfo : uint
    {
        SvmPointers = 0x11B6,
        SvmFineGrainSystem = 0x11B7
    }

    public enum KernelInfo : uint
    {
        FunctionName = 0x1190,
        NumberOfArguments = 0x1191,
        ReferenceCount = 0x1192,
        Context = 0x1193,
        Program = 0x1194,
        Attributes = 0x1195,
        MaxNumberOfSubGroups = 0x11B9,
        CompileNumberOfSubGroups = 0x11BA
    }

    public enum KernelArgInfo : uint
    {
        AddressQualifier = 0x1196,
        AccessQualifier = 0x1197,
        TypeName = 0x1198,
        TypeQualifier = 0x1199,
        Name = 0x119A
    }

    public enum KernelWorkGroupInfo : uint
    {
        WorkGroupSize = 0x11B0,
        CompileWorkGroupSize = 0x11B1,
        LocalMemorySize = 0x11B2,
        PreferredWorkGroupSizeMultiple = 0x11B3,
        PrivateMemorySize = 0x11B4,
        GlobalWorkSize = 0x11B5
    }

    public enum KernelSubGroupInfo : uint
    {
        MaximumSubGroupSizeForNdRange = 0x2033,
        SubGroupCountForNdRange = 0x2034,
        LocalSizeForSubGroupCount = 0x11B8
    }

    public enum EventInfo : uint
    {
        CommandQueue = 0x11D0,
        CommandType = 0x11D1,
        ReferenceCount = 0x11D2,
        CommandExecutionStatus = 0x11D3,
        Context = 0x11D4
    }

    public enum CommandExecutionStatus : int
    {
        Error = -0x1,
        Complete = 0x0,
        Running = 0x1,
        Submitted = 0x2,
        Queued = 0x3
    }

    public enum ProfilingInfo : uint
    {
        CommandQueued = 0x1280,
        CommandSubmit = 0x1281,
        CommandStart = 0x1282,
        CommandEnd = 0x1283,
        CommandComplete = 0x1284
    }

    [Flags]
    public enum MapFlags : ulong
    {
        Read = 1,
        Write = 2,
        WriteInvalidateRegion = 3
    }

    [Flags]
    public enum MemoryMigrationFlags : ulong
    {
        Host = 1,
        ContentUndefined = 2
    }

    #region Unfinished
    public enum DevicePartitionProperty : uint
    {
        Equally,
        ByCounts,
        ByAffinityDomain
    }

    public enum DeviceLocalMemoryType : uint
    {
        Local,
        Global
    }

    public enum DeviceExecutionCapabilities : uint
    {
        Kernel,
        NativeKernel
    }

    public enum DeviceMemoryCacheType : uint
    {
        None,
        ReadOnly,
        ReadWrite
    }

    [Flags]
    public enum DeviceFloatingPointConfig : uint
    {
        Denorm,
        InfinityNaN,
        RoundToNearest,
        RoundToZero,
        RoundToInfinity,
        FusedMultiplyAdd, // IEEE754-2008
        SoftFloat,
        CorrectlyRoundedDivideSqrt
    }

    [Flags]
    public enum CommandQueueProperty : uint
    {
        OutOfOrderExecutionModeEnable,
        ProfilingEnable
    }

    [Flags]
    public enum DeviceAffinityDomain : uint
    {
        Numa,
        L4Cache,
        L3Cache,
        L2Cache,
        L1Cache,
        NextPartionable
    }

    [Flags]
    public enum DeviceSvmCapabilities : uint
    {
        CoarseGrainBuffer,
        FineGrainBuffer,
        FineGrainSystem,
        Atomics
    }

    #endregion
    /// <summary>
    /// Bit field to describe the atomic memory capabilities of a device.
    /// </summary>
    [Flags]
    public enum DeviceAtomicCapabilities : uint
    {
        /// <summary>
        /// Support for the <b>relaxed</b> memory order.
        /// </summary>
        OrderRelaxed = 1,

        /// <summary>
        /// Support for the <b>acquire</b>, <b>release</b>, and <b>acquire-release</b> memory orders.
        /// </summary>
        OrderAcquireRelease = 2,

        /// <summary>
        /// Support for the <b>sequentially consistent</b> memory order.
        /// </summary>
        OrderSequentiallyConsistent = 3,

        /// <summary>
        /// Support for memory ordering constraints that apply to a single work-item.
        /// </summary>
        ScopeWorkItem = 4,

        /// <summary>
        /// Support for memory ordering constraints that apply to all work-items in a work-group.
        /// </summary>
        ScopeWorkGroup = 5,

        /// <summary>
        /// Support for memory ordering constraints that apply to all work-items executing on the device.
        /// </summary>
        ScopeDevice = 6,

        /// <summary>
        /// Support for memory ordering constraints that apply to all work-items
        /// executing across all devices that can share SVM memory with each other and the host process.
        /// </summary>
        SccopeAllDevices = 7,
    }

    /// <summary>
    /// Bitfield that describes device-side enqueue capabilities of the device.
    /// </summary>
    [Flags]
    public enum DeviceEnqueueCapabilities
    {
        /// <summary>
        /// Device supports device-side enqueue and on-device queues.
        /// </summary>
        QueueSupported = 1,

        /// <summary>
        /// Device supports a replaceable default on-device queue.
        /// </summary>
        QueueReplaceableDefault = 2
    }
}
