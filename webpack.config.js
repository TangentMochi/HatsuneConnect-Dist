module.exports = {
    mode: "development",
    entry: `${__dirname}/Assets/JsApi/main.ts`,
    output: {
        path: `${__dirname}/Assets/WebGLTemplates/Penguin/JsApi`,
        filename: "main.js",
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: 'ts-loader',
                exclude: /node_modules/
            },
        ],
    },
    resolve: {
        extensions: [
            '.ts', '.js',
        ],
    },
};