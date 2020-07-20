export function saveUser(state, payload) {
  state.user = payload;
}

export function changeSubscriptionState(state, payload) {
  state.thereIsASubscription = payload;
}
