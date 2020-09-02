const path = require('path');
const webpack = require('webpack');
const TsconfigPathsPlugin = require('tsconfig-paths-webpack-plugin');

module.exports = {
    entry: path.join(__dirname, './../boot-client.tsx'),
    resolve: {
        plugins: [
            new TsconfigPathsPlugin({
                configFile: path.join(__dirname, './../../tsconfig.json'),
            }),
        ],
        extensions: ['.js', '.jsx', '.ts', '.tsx', '.css', '.less'],
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: {
                    loader: 'ts-loader',
                    options: {
                        transpileOnly: true,
                    },
                },
            },
            {
                test: /\.(css|less)(\?|$)/,
                include: path.join(__dirname, './../'),
                use: [
                    'style-loader',
                    'css-modules-typescript-loader',
                    {
                        loader: 'css-loader',
                        options: {
                            modules: true,
                            camelCase: true,
                            // minimize: false,
                            localIdentName: '[name]__[local]',
                        },
                    },
                    'less-loader',
                ],
            },
            {
                test: /\.css$/,
                include: path.join(__dirname, './../../node_modules/@wojtekmaj/react-daterange-picker'),
                use: ['style-loader', 'css-loader'],
            },
            {
                test: /\.css$/,
                include: path.join(__dirname, './../../node_modules/react-calendar'),
                use: ['style-loader', 'css-loader'],
            },
            {
                test: /\.css$/,
                include: path.join(__dirname, './../../node_modules/react-table'),
                use: ['style-loader', 'css-loader'],
            },
            {
                test: /\.css$/,
                include: path.join(__dirname, './../../node_modules/react-vis/dist'),
                use: ['style-loader', 'css-loader'],
            },
        ],
    },
    plugins: [new webpack.IgnorePlugin(/\.(png|jpg|jpeg|gif|svg|ttf)$/)],
};
