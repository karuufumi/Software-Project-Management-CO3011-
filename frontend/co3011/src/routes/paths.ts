
export const rootPaths = {
  userRoot: "/user",
  libRoot: "/librarian",
  authRoot: "/authentication",
  adminRoot: "/admin",
  guestRoot: "/", 
};

export default {
  GUEST: { 
    HISTORY: rootPaths.guestRoot,        // "/"
    DASHBOARD: rootPaths.guestRoot,      // "/"
    LIBRARY_CATALOG: `${rootPaths.guestRoot}catalog`,  // "/catalog"
    ABOUT: `${rootPaths.guestRoot}about`,             // "/about"
  },

  USER: {
    DASHBOARD: rootPaths.userRoot,                          // "/user"
    LIBRARY_CATALOG: `${rootPaths.userRoot}/catalog`,       // "/user/catalog"
    BOOK_DETAIL: `${rootPaths.userRoot}/catalog/book/:bookid`, // "/user/catalog/book/:bookid"
    BOOK_CONTRIBUTE: `${rootPaths.userRoot}/catalog/contribute`, // "/user/catalog/contribute"
    HISTORY: `${rootPaths.userRoot}/history`,               // "/user/history"
    PROFILE: `${rootPaths.userRoot}/profile`,               // "/user/profile"
    RANKMAP: `${rootPaths.userRoot}/rankmap`,               // "/user/rankmap"
    LEADERBOARD: `${rootPaths.userRoot}/leaderboard`,       // "/user/leaderboard"
  },

  LIBRARIAN: {
    DASHBOARD: rootPaths.libRoot,                          // "/librarian"
    BOOK_MANAGEMENT: `${rootPaths.libRoot}/book`,          // "/librarian/book"
    HISTORY: `${rootPaths.libRoot}/history`,               // "/librarian/history"
    MISSING_BOOK_HANDLER: `${rootPaths.libRoot}/missing-book`, // "/librarian/missing-book"
  },

  ADMIN: {
    DASHBOARD: rootPaths.adminRoot,                        // "/admin"
    MEMBER_MANAGEMENT: `${rootPaths.adminRoot}/member`,    // "/admin/member"
    REPORT_AND_ANALYTIC: `${rootPaths.adminRoot}/report`,  // "/admin/report"
  },
};
