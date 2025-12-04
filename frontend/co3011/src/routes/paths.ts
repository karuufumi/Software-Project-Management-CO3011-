export const rootPaths = {
  userRoot: "/user",
  libRoot: "/librarian",
  authRoot: "/authentication",
  adminRoot: "/admin",
  guestRoot: "/", 
};

export default {
  GUEST: { 
    //! HISTORY OF GUEST NOT IMPLEMENTED YET
    // HISTORY: `${rootPaths.guestRoot}/history`,
    HISTORY: rootPaths.guestRoot,
    DASHBOARD: rootPaths.guestRoot,
    LIBRARY_CATALOG: `${rootPaths.guestRoot}/catalog`,
    ABOUT: `${rootPaths.guestRoot}/about`,
  },
  USER: {
    DASHBOARD: rootPaths.userRoot,
    LIBRARY_CATALOG: `${rootPaths.userRoot}catalog`,
    BOOK_DETAIL: `${rootPaths.userRoot}catalog/book/:bookid`,
    BOOK_CONTRIBUTE: `${rootPaths.userRoot}catalog/contribute`,
    HISTORY: `${rootPaths.userRoot}history`,
    PROFILE: `${rootPaths.userRoot}profile`,
    RANKMAP: `${rootPaths.userRoot}rankmap`,
    LEADERBOARD: `${rootPaths.userRoot}leaderboard`, 
  },
  LIBRARIAN: {
    DASHBOARD: rootPaths.libRoot,
    BOOK_MANAGEMENT: `${rootPaths.libRoot}/book`,
    HISTORY: `${rootPaths.libRoot}/history`,
    MISSING_BOOK_HANDLER: `${rootPaths.libRoot}/missing-book`,
  },
  ADMIN: {
    DASHBOARD: rootPaths.adminRoot,
    MEMBER_MANAGEMENT: `${rootPaths.adminRoot}/member`,
    REPORT_AND_ANALYTIC: `${rootPaths.adminRoot}/report`,
  },
};