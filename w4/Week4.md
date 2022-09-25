# Example

FF():
    let f(e)=0 forall edges
    while s->t path in G_residual
        let P be a simple path in G_residual
        augment along path P
        update f to be f'
        Update G_residual
    end
end
