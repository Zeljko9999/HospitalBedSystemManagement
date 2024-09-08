export const TOKEN_REFRESH_FAILED_NAME = "TokenRefreshFailedError";

export class TokenRefreshError extends Error {
  constructor(message?: string) {
    super(message || "Token refresh failed");
    this.name = TOKEN_REFRESH_FAILED_NAME;
  }
}
