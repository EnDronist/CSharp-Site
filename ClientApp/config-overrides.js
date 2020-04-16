/* config-overrides.js */

module.exports = {
    webpack: function(config, env) {
        // ...add your webpack config
        return config;
    },
    // The paths config to use when compiling your react app for development or production.
    paths: function(paths, env) {
        // ...add your paths config
        // paths = {
        //     "*": ["node_modules/*"],
        //     "@api/*": ["src/api/*"],
        //     "@store/*": ["src/store/*"],
        //     "@components/*": ["src/components/*"],
        //     "@utils/*": ["src/utils/*"]
        // }
        return paths;
    },
}