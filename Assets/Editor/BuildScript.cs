using UnityEditor;

public static class BuildScript {
    public static void BuildWebGL() {
        // Disable compression
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;

        // Build WebGL
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "BuildOutputPath", BuildTarget.WebGL, BuildOptions.None);
    }
}