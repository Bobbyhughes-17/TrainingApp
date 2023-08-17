import { createContext } from 'react';

const UserContext = createContext({
    user: null,
    isAuthenticated: false,
    
    loginUser: () => {},
    logoutUser: () => {},
    setIsAuthenticated: () => {},
    setUser: () => {}
});

export default UserContext;
