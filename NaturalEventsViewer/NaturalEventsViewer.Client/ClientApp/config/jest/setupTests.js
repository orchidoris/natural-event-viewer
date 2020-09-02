const util = require('util');
const crypto = require('crypto');

// setup file
import { configure } from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

configure({ adapter: new Adapter() });

util.inspect.defaultOptions.customInspect = false;

class MockHeader {}

global.Headers = MockHeader;

Object.defineProperty(global.self, 'crypto', {
    value: {
        getRandomValues: (arr) => crypto.randomBytes(arr.length),
    },
});
