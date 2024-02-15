using System;
using System.Runtime.InteropServices;

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
    /// The information that can be queried using
    /// <c><see cref="CL.GetDeviceInfo(CLDevice, DeviceInfo, out byte[])">GetDeviceInfo()</see></c>.
    /// <para>
    /// Original documentation: https://registry.khronos.org/OpenCL/specs/3.0-unified/html/OpenCL_API.html#device-queries-table.
    /// </para>
    /// </summary>
    public enum DeviceInfo : ulong
    {
        /// <summary>
        /// The type or types of the OpenCL device. Please see <c><see cref="DeviceType">DeviceType</see></c>
        /// for supported device types and device type combinations.
        /// </summary>
        /// <remarks>Return Type: <c><see cref="DeviceType">DeviceType</see></c></remarks>
        Type = 0x1000,

        /// <summary>
        /// A unique device vendor identifier.
        /// <para>If the vendor has a PCI vendor ID, the low 16 bits must contain that PCI vendor ID,
        /// and the remaining bits must be set to zero. Otherwise, the value returned must be a
        /// valid Khronos vendor ID represented by type cl_khronos_vendor_id.
        /// Khronos vendor IDs are allocated starting at 0x10000, to distinguish them from the PCI vendor ID namespace.</para>
        /// </summary>
        /// <remarks>Return Type: uint</remarks>
        VendorId = 0x1001,

        /// <summary>
        /// The number of parallel compute units on the OpenCL device.
        /// A work-group executes on a single compute unit. The minimum value is 1.
        /// </summary>
        /// <remarks>Return Type: uint</remarks>
        MaximumComputeUnits = 0x1002,

        /// <summary>
        /// Maximum dimensions that specify the global and local work-item IDs used by the data parallel execution model.
        /// (Refer to
        /// <c><see cref="CL.EnqueueNDRangeKernel(CLCommandQueue,
        /// CLKernel, uint, UIntPtr[], UIntPtr[], UIntPtr[], uint,
        /// CLEvent[], out CLEvent)">EnqueueNDRangeKernel()</see></c>).
        /// The minimum value is 3 for devices that are not of type
        /// <c><see cref="DeviceType.Custom">DeviceType.Custom</see></c>.
        /// </summary>
        /// <remarks>Return Type: UIntPtr</remarks>
        MaximumWorkItemDimensions = 0x1003,

        /// <summary>
        /// Maximum number of work-items in a work-group that a device is capable
        /// of executing on a single compute unit, for any given kernel-instance running on the device.
        /// (Refer to <c><see cref="CL.EnqueueNDRangeKernel(CLCommandQueue,
        /// CLKernel, uint, UIntPtr[], UIntPtr[], UIntPtr[], uint,
        /// CLEvent[], out CLEvent)">EnqueueNDRangeKernel()</see></c> and
        /// <c><see cref="KernelWorkGroupInfo.WorkGroupSize">WorkGroupSize</see></c>).
        /// The minimum value is 1.
        /// <para>The returned value is an upper limit and will not necessarily maximize performance.
        /// This maximum may be larger than supported by a specific kernel
        /// (refer to the <c><see cref="KernelWorkGroupInfo.WorkGroupSize">WorkGroupSize</see></c>
        /// query of <c><see cref="CL.GetKernelWorkGroupInfo(CLKernel, CLDevice,
        /// KernelWorkGroupInfo, UIntPtr, byte[], out UIntPtr)">GetKernelWorkGroupInfo()</see></c>).</para>
        /// </summary>
        /// <remarks>Return Type: UIntPtr</remarks>
        MaximumWorkGroupSize = 0x1004,

        /// <summary>
        /// Maximum number of work-items that can be specified in each dimension of the work-group to
        /// <c><see cref="CL.EnqueueNDRangeKernel(CLCommandQueue,
        /// CLKernel, uint, UIntPtr[], UIntPtr[], UIntPtr[], uint,
        /// CLEvent[], out CLEvent)">EnqueueNDRangeKernel()</see></c>.
        /// Returns n <c>UIntPtr</c> entries, where n is the value returned by the query for
        /// <c><see cref="MaximumWorkItemDimensions">MaximumWorkItemDimensions</see></c>.
        /// The minimum value is (1, 1, 1) for devices that are not of type
        /// <c><see cref="DeviceType.Custom">Custom</see></c>.
        /// </summary>
        /// <remarks>Return Type: UIntPtr[]</remarks>
        MaximumWorkItemSizes = 0x1005,

        /// <summary>
        /// Preferred native vector width size for built-in scalar types that can be put into vectors.
        /// <para>The vector width is defined as the number of scalar elements that can be stored in the vector.</para>
        /// </summary>
        /// <remarks>Return Type: uint</remarks>
        PreferredVectorWidthChar = 0x1006,

        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        PreferredVectorWidthShort = 0x1007,

        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        PreferredVectorWidthInt = 0x1008,

        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        PreferredVectorWidthLong = 0x1009,

        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        PreferredVectorWidthFloat = 0x100A,

        /// <remarks>
        /// If double precision is not supported, must return 0.
        /// <para>Return Type: uint</para>
        /// </remarks>
        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        PreferredVectorWidthDouble = 0x100B,

        /// <summary>
        /// <list type="bullet">
        /// <item>
        /// <term>Before OpenCL 2.2</term>
        /// <description>Maximum configured clock frequency of the device in MHz.</description>
        /// </item>
        ///
        /// <item>
        /// <term>After OpenCL 2.2</term>
        /// <description>Clock frequency of the device in MHz.
        /// The meaning of this value is implementation-defined.
        /// For devices with multiple clock domains,
        /// the clock frequency for any of the clock domains may be returned.
        /// For devices that dynamically change frequency for power or thermal reasons,
        /// the returned clock frequency may be any valid frequency.</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <remarks>Return Type: uint</remarks>
        MaximumClockFrequency = 0x100C,

        /// <summary>
        /// The default compute device address space size of the global
        /// address space specified as an unsigned integer value in bits.
        /// Currently supported values are 32 or 64 bits.
        /// </summary>
        /// <remarks>Return Type: uint</remarks>
        AddressBits = 0x100D,

        /// <summary>
        /// Max number of image objects arguments of a kernel
        /// declared with the read_only qualifier.
        /// The minimum value is 128 if <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE, the value is 0 otherwise.
        /// </summary>
        /// <remarks>Return Type: uint</remarks>
        MaximumReadImageArguments = 0x100E,

        /// <summary>
        /// Max number of image objects arguments of a kernel
        /// declared with the write_only qualifier.
        /// The minimum value is 64 if <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE, the value is 0 otherwise.
        /// </summary>
        /// <remarks>Return Type: uint</remarks>
        MaximumWriteImageArguments = 0x100F,

        /// <summary>
        /// Max size of memory object allocation in bytes.
        /// The minimum value is:
        /// <para><code>max(min(1024 × 1024 × 1024, 1/4th of <see cref="GlobalMemorySize">DeviceInfo.GlobalMemorySize</see>), 32 × 1024 × 1024)
        /// </code></para>
        /// for devices that are not of type <c><see cref="DeviceType.Custom">DeviceType.Custom</see></c>.
        /// </summary>
        /// <remarks>Return Type: ulong</remarks>
        MaximumMemoryAllocationSize = 0x1010,

        /// <summary>
        /// Max width of 2D image or 1D image not created from a buffer object in pixels.
        /// The minimum value is 16384 if <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE,
        /// the value is 0 otherwise.
        /// </summary>
        /// <remarks>Return Type: UIntPtr</remarks>
        Image2DMaximumWidth = 0x1011,

        /// <summary>
        /// Max height of 2D image in pixels.
        /// The minimum value is 16384 if <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE,
        /// the value is 0 otherwise.
        /// </summary>
        /// <remarks>Return Type: UIntPtr</remarks>
        Image2DMaximumHeight = 0x1012,

        /// <summary>
        /// Max width of 3D image in pixels.
        /// The minimum value is 2048 if <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE,
        /// the value is 0 otherwise.
        /// </summary>
        /// <remarks>Return Type: UIntPtr</remarks>
        Image3DMaximumWidth = 0x1013,

        /// <summary>
        /// Max height of 3D image in pixels.
        /// The minimum value is 2048 if <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE,
        /// the value is 0 otherwise.
        /// </summary>
        /// <remarks>Return Type: UIntPtr</remarks>
        Image3DMaximumHeight = 0x1014,

        /// <summary>
        /// Max depth of 3D image in pixels.
        /// The minimum value is 2048 if <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE,
        /// the value is 0 otherwise.
        /// </summary>
        /// <remarks>Return Type: UIntPtr</remarks>
        Image3DMaximumDepth = 0x1015,

        /// <summary>
        /// Is TRUE if images are supported by the OpenCL device and FALSE otherwise.
        /// </summary>
        /// <remarks>Return Type: bool</remarks>
        ImageSupport = 0x1016,

        /// <summary>
        /// Max size in bytes of all arguments that can be passed to a kernel.
        /// The minimum value is 1024 for devices that are not of type
        /// <c><see cref="DeviceType.Custom">DeviceType.Custom</see></c>.
        /// For this minimum value, only a maximum of 128 arguments can be passed to a kernel.
        /// For all other values, a maximum of 255 arguments can be passed to a kernel.
        /// </summary>
        /// <remarks>Return Type: UIntPtr</remarks>
        MaximumParameterSize = 0x1017,

        /// <summary>
        /// Maximum number of samplers that can be used in a kernel.
        /// The minimum value is 16 if <c><see cref="ImageSupport">ImageSupport</see></c>
        /// is TRUE, the value is 0 otherwise.
        /// </summary>
        /// <remarks>Return Type: uint</remarks>
        MaximumSamplers = 0x1018,

        /// <summary>
        /// Alignment requirement (in bits) for sub-buffer offsets.
        /// The minimum value is the size (in bits) of the largest OpenCL built-in data type
        /// supported by the device (long16 in FULL profile, long16 or int16 in EMBEDDED profile)
        /// for devices that are not of type <c><see cref="DeviceType.Custom">DeviceType.Custom</see></c>.
        /// </summary>
        /// <remarks>Return Type: uint</remarks>
        MemoryBaseAddressAlignment = 0x1019,

        /// <summary>
        /// Deprecated OpenCL 1.1 property
        /// <para>The minimum value is the size (in bytes)
        /// of the largest OpenCL data type supported by the device
        /// (long16 in FULL profile, long16 or int16 in EMBEDDED profile).</para>
        /// </summary>
        /// <remarks>Return Type: uint</remarks>
        [Obsolete("MinimumDataTypeAlignmentSize is a deprecated OpenCL 1.1 property.")]
        MinimumDataTypeAlignmentSize = 0x101A,

        /// #TODO: Missing return type equivalent in the API
        /// <remarks>Return Type: cl_device_fp_config</remarks>
        SingleFloatingPointConfiguration = 0x101B,

        /// #TODO: Missing return type equivalent in the API
        /// <summary>
        /// Type of global memory cache supported.
        /// Valid values are: CL_NONE, CL_READ_ONLY_CACHE, and CL_READ_WRITE_CACHE.
        /// </summary>
        /// <remarks>Return Type: cl_device_mem_cache_type</remarks>
        GlobalMemoryCacheType = 0x101C,

        /// <summary>
        /// Size of global memory cache line in bytes.
        /// </summary>
        /// <remarks>Return Type: uint</remarks>
        GlobalMemoryCachelineSize = 0x101D,

        /// <summary>
        /// Size of global memory cache in bytes.
        /// </summary>
        /// <remarks>Return Type: ulong</remarks>
        GlobalMemoryCacheSize = 0x101E,

        /// <summary>
        /// Size of global device memory in bytes.
        /// </summary>
        /// <remarks>Return Type: ulong</remarks>
        GlobalMemorySize = 0x101F,

        /// <summary>
        /// Max size in bytes of a constant buffer allocation. The minimum value is 64 KB.
        /// </summary>
        /// <remarks>Return Type: ulong</remarks>
        MaximumConstantBufferSize = 0x1020,

        /// <summary>
        /// Max number of arguments declared with the <c>__constant</c> qualifier in a kernel. The minimum value is 8.
        /// </summary>
        /// <remarks>Return Type: uint</remarks>
        MaximumConstantArguments = 0x1021,

        /// #TODO: Missing return type equivalent in the API
        /// <summary>
        /// Type of local memory supported.
        /// This can be set to CL_LOCAL implying dedicated local memory storage such as SRAM, or CL_GLOBAL.
        /// </summary>
        /// <remarks>Return Type: cl_device_local_mem_type</remarks>
        LocalMemoryType = 0x1022,

        /// <summary>
        /// Size of local memory arena in bytes. The minimum value is 32 KB.
        /// </summary>
        /// <remarks>Return Type: ulong</remarks>
        LocalMemorySize = 0x1023,

        /// <summary>
        /// Is TRUE if the device implements error correction
        /// for all accesses to compute device memory (global and constant).
        /// Is FALSE if the device does not implement such error correction.
        /// </summary>
        /// <remarks>Return Type: bool</remarks>
        ErrorCorrectionSupport = 0x1024,

        /// <summary>
        /// Describes the resolution of device timer. This is measured in nanoseconds.
        /// </summary>
        /// <remarks>Return Type: UIntPtr</remarks>
        ProfilingTimerResolution = 0x1025,

        /// <summary>
        /// Is TRUE if the OpenCL device is a little endian device and FALSE otherwise.
        /// </summary>
        /// <remarks>Return Type: bool</remarks>
        EndianLittle = 0x1026,

        /// <summary>
        /// Is TRUE if the device is available and FALSE if the device is not available.
        /// </summary>
        /// <remarks>Return Type: bool</remarks>
        Available = 0x1027,

        /// <summary>
        /// Is FALSE if the implementation does not have
        /// a compiler available to compile the program source.
        /// Is TRUE if the compiler is available.
        /// This can be FALSE for the embedded platform profile only.
        /// </summary>
        /// <remarks>Return Type: bool</remarks>
        CompilerAvailable = 0x1028,

        /// #TODO: Missing return type equivalent in the API
        /// <summary>
        /// Describes the execution capabilities of the device.
        /// This is a bit-field that describes one or more of the following values:
        /// <list type="bullet">
        /// <item><term>
        /// CL_EXEC_KERNEL</term>
        /// <description>
        /// The OpenCL device can execute OpenCL kernels.
        /// </description></item>
        ///
        /// <item><term>
        /// CL_EXEC_NATIVE_KERNEL</term>
        /// <description>
        /// The OpenCL device can execute native kernels.
        /// </description></item>
        /// </list>
        ///
        /// The mandated minimum capability is CL_EXEC_KERNEL.
        /// </summary>
        /// <remarks>Return Type: cl_device_exec_capabilities</remarks>
        ExecutionCapabilities = 0x1029,

        /// #TODO: Unfinished return type in API
        /// <summary>
        /// Describes the command-queue properties supported by the device.
        /// The mandated minimum capability is CL_QUEUE_PROFILING_ENABLE.
        /// </summary>
        /// <remarks>Return Type: <c><see cref="CommandQueueProperty">CommandQueueProperty</see></c></remarks>
        [Obsolete("QueueProperties is a deprecated OpenCL 1.2 property, please use QueueOnHostProperties.")]
        QueueProperties = 0x102A,

        /// #TODO: Unfinished return type in API
        /// <summary>
        /// Describes the on host command-queue properties supported by the device.
        /// The mandated minimum capability is: CL_QUEUE_PROFILING_ENABLE.
        /// </summary>
        /// <remarks>Return Type: <c><see cref="CommandQueueProperty">CommandQueueProperty</see></c></remarks>
        QueueOnHostProperties = 0x102A,

        /// <summary>
        /// Device name string.
        /// </summary>
        /// <remarks>Return Type: string</remarks>
        Name = 0x102B,

        /// <summary>
        /// Vendor name string.
        /// </summary>
        /// <remarks>Return Type: string</remarks>
        Vendor = 0x102C,

        /// <summary>
        /// OpenCL software driver version string. Follows a vendor-specific format.
        /// </summary>
        /// <remarks>Return Type: string</remarks>
        DriverVersion = 0x102D,

        /// <summary>
        /// OpenCL profile string.
        /// Returns the profile name supported by the device.
        /// The profile name returned can be one of the following strings:
        /// <list type="bullet">
        /// <item><term>
        /// FULL_PROFILE</term>
        /// <description>
        /// if the device supports the OpenCL specification
        /// (functionality defined as part of the core specification and does not require any extensions to be supported).
        /// </description></item>
        ///
        /// <item><term>
        /// EMBEDDED_PROFILE</term>
        /// <description>
        /// if the device supports the OpenCL embedded profile.
        /// </description></item>
        /// </list>
        /// </summary>
        /// <remarks>Return Type: string</remarks>
        Profile = 0x102E,

        /// <summary>
        /// OpenCL version string. Returns the OpenCL version supported by the device. This version string has the following format:
        /// <para><c>OpenCL &lt;space&gt;&lt;major_version.minor_version&gt;&lt;space&gt;&lt;vendor-specific information&gt;</c></para>
        /// The <c>major_version.minor_version</c> value returned will be one of 1.0, 1.1, 1.2, 2.0, 2.1, 2.2, or 3.0.
        /// </summary>
        /// <remarks>Return Type: string</remarks>
        Version = 0x102F,

        /// <summary>
        /// Returns a space separated list of extension names
        /// (the extension names themselves do not contain any spaces) supported by the device.
        /// The list of extension names may include Khronos approved extension names and vendor specified extension names.
        /// <para>The following Khronos extension names must be returned by all devices that support OpenCL 1.1:</para>
        ///
        /// <list type="bullet">
        /// <item><description>
        /// cl_khr_byte_addressable_store</description>
        /// </item>
        ///
        /// <item><description>
        /// cl_khr_global_int32_base_atomics</description>
        /// </item>
        ///
        /// <item><description>
        /// cl_khr_local_int32_base_atomics</description>
        /// </item>
        ///
        /// <item><description>
        /// cl_khr_local_int32_extended_atomics</description>
        /// </item>
        /// </list>
        ///
        /// Additionally, the following Khronos extension names must be returned by all devices
        /// that support OpenCL 1.2 when and only when the optional feature is supported:
        /// <list type="bullet">
        /// <item><description>
        /// cl_khr_fp64</description>
        /// </item>
        /// </list>
        ///
        /// Additionally, the following Khronos extension names must be returned
        /// by all devices that support OpenCL 2.0, OpenCL 2.1, or OpenCL 2.2.
        /// For devices that support OpenCL 3.0,
        /// these extension names must be returned when and only when the optional feature is supported:
        ///
        /// <list type="bullet">
        /// <item><description>
        /// cl_khr_3d_image_writes</description>
        /// </item>
        ///
        /// <item><description>
        /// cl_khr_depth_images</description>
        /// </item>
        ///
        /// <item><description>
        /// cl_khr_image2d_from_buffer</description>
        /// </item>
        /// </list>
        ///
        /// Please refer to the OpenCL Extension Specification or vendor provided documentation
        /// for a detailed description of these extensions.
        /// </summary>
        /// <remarks>Return Type: string</remarks>
        Extensions = 0x1030,

        /// <summary>
        /// The platform associated with this device.
        /// </summary>
        /// <remarks>Return Type: <c><see cref="CLPlatform">CLPlatform</see></c></remarks>
        Platform = 0x1031,

        /// #TODO: Missing return type equivalent in the API
        /// <summary>
        /// Describes double precision floating-point capability of the OpenCL device.
        /// </summary>
        /// <remarks>Return Type: cl_device_fp_config</remarks>
        DoubleFloatingPointConfiguration = 0x1032,

        /// #TODO: Missing return type equivalent in the API
        /// <summary>
        /// Describes the OPTIONAL half precision floating-point capability of the OpenCL device.
        /// </summary>
        /// <remarks>Return Type: cl_device_fp_config</remarks>
        HalfFloatingPointConfiguration = 0x1033,

        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        PreferredVectorWidthHalf = 0x1034,

        /// <summary>
        /// Is TRUE if the device and the host have a unified memory subsystem and is FALSE otherwise.
        /// </summary>
        /// <remarks>Return Type: bool</remarks>
        [Obsolete("HostUnifiedMemory is a deprecated OpenCL 1.2 property.")]
        HostUnifiedMemory = 0x1035,

        /// <summary>
        /// Returns the native ISA vector width.
        /// The vector width is defined as the number of scalar elements that can be stored in the vector.
        /// </summary>
        /// <remarks>Return Type: uint</remarks>
        NativeVectorWidthChar = 0x1036,

        /// <inheritdoc cref="NativeVectorWidthChar"/>
        NativeVectorWidthShort = 0x1037,

        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        NativeVectorWidthInt = 0x1038,

        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        NativeVectorWidthLong = 0x1039,

        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        NativeVectorWidthFloat = 0x103A,

        /// <remarks>If double precision is not supported, must return 0.
        /// <para>Return Type: uint</para>
        /// </remarks>
        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        NativeVectorWidthDouble = 0x103B,

        /// <remarks>If the <c>cl_khr_fp16</c> extension is not supported, must return 0.
        /// <para>Return Type: uint</para>
        /// </remarks>
        /// <inheritdoc cref="PreferredVectorWidthChar"/>
        NativeVectorWidthHalf = 0x103C,

        /// #TODO: missing 3.0 implementation
        /// <summary>
        /// Unavailable before version 1.1 and obsolete after version 3.0.
        /// <para>
        /// Returns the highest fully backwards compatible OpenCL C version
        /// supported by the compiler for the device.
        /// For devices supporting compilation from OpenCL C source,
        /// this will return a version string with the following format:
        /// </para>
        /// <para>
        /// <c>OpenCL &lt;space&gt;C&lt;space&gt;&lt;major_version.minor_version&gt;&lt;space&gt;&lt;vendor-specific information&gt;</c>
        /// </para>
        /// <para>
        /// For devices that support compilation from OpenCL C source:
        /// </para>
        /// <para>
        /// Because OpenCL 3.0 is backwards compatible with OpenCL C 1.2,
        /// an OpenCL 3.0 device must support at least OpenCL C 1.2. An OpenCL 3.0
        /// device may return an OpenCL C version newer than OpenCL C 1.2 if and only
        /// if all optional OpenCL C features are supported by the device for the newer version.
        /// </para>
        /// <para>
        /// Because OpenCL 3.0 is backwards compatible with OpenCL C 1.2,
        /// an OpenCL 3.0 device must support at least OpenCL C 1.2.
        /// An OpenCL 3.0 device may return an OpenCL C version newer than
        /// OpenCL C 1.2 if and only if all optional OpenCL C features are
        /// supported by the device for the newer version.
        /// </para>
        /// <para>
        /// Support for OpenCL C 2.0 is required for an OpenCL 2.0, OpenCL 2.1, or OpenCL 2.2 device.
        /// </para>
        /// <para>
        /// Support for OpenCL C 1.2 is required for an OpenCL 1.2 device.
        /// </para>
        /// <para>
        /// Support for OpenCL C 1.1 is required for an OpenCL 1.1 device.
        /// </para>
        /// <para>
        /// Support for either OpenCL C 1.0 or OpenCL C 1.1 is required for an OpenCL 1.0 device.
        /// </para>
        /// <para>
        /// For devices that do not support compilation from OpenCL C source,
        /// such as when <c><see cref="CompilerAvailable">CompilerAvailable</see></c> is FALSE, this query may return an empty string.
        /// </para>
        /// <para>
        /// This query has been superseded by the CL_DEVICE_OPENCL_C_ALL_VERSIONS query, which returns a set of OpenCL C versions supported by a device.
        /// </para>
        /// </summary>
        /// <remarks>>Return Type: string</remarks>
        OpenClCVersion = 0x103D,

        /// <summary>
        /// Is FALSE if the implementation does not have a linker available. Is TRUE if the linker is available.
        /// This can be FALSE for the embedded platform profile only.
        /// This must be TRUE if <c><see cref="CompilerAvailable">CompilerAvailable</see></c> is TRUE.
        /// </summary>
        /// <remarks>Return Type: bool</remarks>
        LinkerAvailable = 0x103E,

        /// <summary>
        /// Unavailable before version 1.2.
        /// <para>
        /// A semi-colon separated list of built-in kernels supported by the device.
        /// An empty string is returned if no built-in kernels are supported by the device.
        /// </para>
        /// </summary>
        /// <remarks>Return Type: string</remarks>
        BuiltInKernels = 0x103F,

        /// <summary>
        /// Unavailable before version 1.2.
        /// <para>Max number of pixels for a 1D image created from a buffer object.
        /// The minimum value is 65536 if <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE, the value is 0 otherwise.
        /// </para>
        /// </summary>
        /// <remarks>Return Type: UIntPtr</remarks>
        ImageMaximumBufferSize = 0x1040,

        /// <summary>
        /// Unavailable before version 1.2.
        /// <para>Max number of images in a 1D or 2D image array.
        /// The minimum value is 2048 <c><see cref="ImageSupport">ImageSupport</see></c> is TRUE, the value is 0 otherwise.
        /// </para>
        /// </summary>
        /// <remarks>Return Type: UIntPtr</remarks>
        ImageMaximumArraySize = 0x1041,
        ParentDevice = 0x1042,
        PartitionMaximumSubDevices = 0x1043,
        PartitionProperties = 0x1044,
        PartitionAffinityDomain = 0x1045,
        PartitionType = 0x1046,
        ReferenceCount = 0x1047,
        PreferredInteropUserSync = 0x1048,
        PrintfBufferSize = 0x1049,
        ImagePitchAlignment = 0x104A,
        ImageBaseAddressAlignment = 0x104B,
        MaximumReadWriteImageArguments = 0x104C,
        MaximumGlobalVariableSize = 0x104D,
        QueueOnDeviceProperties = 0x104E,
        QueueOnDevicePreferredSize = 0x104F,
        QueueOnDeviceMaximumSize = 0x1050,
        MaximumOnDeviceQueues = 0x1051,
        MaximumOnDeviceEvents = 0x1052,
        SvmCapabilities = 0x1053,
        GlobalVariablePreferredTotalSize = 0x1054,
        MaximumPipeArguments = 0x1055,
        PipeMaximumActiveReservations = 0x1056,
        PipeMaximumPacketSize = 0x1057,
        PreferredPlatformAtomicAlignment = 0x1058,
        PreferredGlobalAtomicAlignment = 0x1059,
        PreferredLocalAtomicAlignment = 0x105A,
        IntermediateLanguageVersion = 0x105B,
        MaximumNumberOfSubGroups = 0x105C,
        SubGroupIndependentForwardProgress = 0x105D,
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
        Read = 1 << 0,
        Write = 1 << 1,
        WriteInvalidateRegion = 1 << 2
    }

    [Flags]
    public enum MemoryMigrationFlags : ulong
    {
        Host = 1 << 0,
        ContentUndefined = 1 << 1
    }

    [Flags]
    public enum CommandQueueProperty : ulong
    {
    }
}
